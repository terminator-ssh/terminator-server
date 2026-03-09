using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Terminator.Application.Common;
using Terminator.Application.Common.Options;
using Terminator.Core.Common.Errors;
using Terminator.Core.Entities;
using Entities =  Terminator.Core.Entities;
using Terminator.Core.Result;

namespace Terminator.Application.Features.Sync;

public class Handler(
    IApplicationDbContext db, 
    TimeProvider timeProvider,
    IOptions<UserOptions> options,
    ILogger<Handler> logger) : IRequestHandler<Request, Result<Response>>
{
    private readonly UserOptions _userOptions = options.Value;
    
    public async Task<Result<Response>> Handle(Request request, CancellationToken cancellationToken)
    {
        var user = await db.Users
            .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
        
        if (user is null)
        {
            return Result<Response>.Error(ErrorType.Validation, DomainErrors.User.NotFound);
        }
        
        var clientBlobIds = request.Blobs.Select(x => x.Id).ToList();
        var clientBlobsById = 
            request.Blobs
                .DistinctBy(x => x.Id) 
                .ToDictionary(x => x.Id);
        
        var serverBlobTimestampsById = await db.EncryptedBlobs
            .Where(x => x.User.Id == request.UserId)
            .Where(x => clientBlobIds.Contains(x.Id))
            .Select(x => new { x.Id, x.UpdatedAt })
            .ToDictionaryAsync(
                x => x.Id, 
                x => x.UpdatedAt,
                cancellationToken);

        
        var blobsToUpdate = new List<EncryptedBlob>();
        var blobsToAdd = new List<EncryptedBlob>();
        
        var clientStaleIds = new List<Guid>(); 
        
        foreach (var pair in clientBlobsById)
        {
            var clientBlobId = pair.Key;
            var clientBlob = pair.Value;
            
            if (!serverBlobTimestampsById
                    .TryGetValue(clientBlobId, out var serverBlobTimestamp))
            {
                var encryptedBlob = MapEncryptedBlob(clientBlob, user);
                // don't filter just in case?
                //if(!encryptedBlob.IsDeleted)
                blobsToAdd.Add(encryptedBlob);
            }

            else if (clientBlob.UpdatedAt > serverBlobTimestamp)
            {
                var encryptedBlob = MapEncryptedBlob(clientBlob, user);
                blobsToUpdate.Add(encryptedBlob);
            }
            
            else if (clientBlob.UpdatedAt < serverBlobTimestamp)
            {
                clientStaleIds.Add(clientBlob.Id);
            }
        }

        int existingActiveBlobCount =
            await db.EncryptedBlobs
                .Where(x => x.User.Id == request.UserId)
                .CountAsync(EncryptedBlob.ActiveBlobFilter, cancellationToken);

        int newActiveBlobCount = blobsToAdd.Count(EncryptedBlob.ActiveBlobFilter.Compile());
        
        int totalAfterSync = existingActiveBlobCount + newActiveBlobCount;

        // TODO figure out good ux that still allows updates to go through
        if (user.HasReachedBlobLimit(totalAfterSync, _userOptions.BlobLimit))
        {
            return Result<Response>.Error(ErrorType.Validation, DomainErrors.User.BlobLimitReached);
        }
        
        db.EncryptedBlobs.AddRange(blobsToAdd);
        db.EncryptedBlobs.UpdateRange(blobsToUpdate);

        await db.SaveChangesAsync(cancellationToken);

        var updatedOrAddedBlobIds = new List<Guid>();
        updatedOrAddedBlobIds.AddRange(blobsToUpdate.Select(x => x.Id));
        updatedOrAddedBlobIds.AddRange(blobsToAdd.Select(x => x.Id));
        
        var newBlobs = await db.EncryptedBlobs
            .Where(x => x.User.Id == request.UserId) 
            .Where(x
                => (x.UpdatedAt > request.LastSyncTime 
                   && !updatedOrAddedBlobIds.Contains(x.Id)) 
                   || clientStaleIds.Contains(x.Id))
            .ToListAsync(cancellationToken);
        
        var newBlobDtos = newBlobs.Select(MapEncryptedBlobDto).ToList();

        logger.LogDebug(
            "Sync complete for user {username}. " +
            "Received {receivedBlobCount} blobs, {staleBlobCount} of them stale. " +
            "Sending {newBlobCount} blobs", 
            user.Username, request.Blobs.Count, clientStaleIds.Count, newBlobDtos.Count);
        
        var response = new Response(newBlobDtos, timeProvider.GetUtcNow());

        return Result<Response>.Success(response);
    }

    // TODO: Mappers
    private EncryptedBlob MapEncryptedBlob(EncryptedBlobDto dto, Entities.User user)
    {
        var iv = Convert.FromBase64String(dto.Iv);
        var blob = Convert.FromBase64String(dto.Blob);

        return new EncryptedBlob(
            dto.Id,
            dto.UpdatedAt,
            dto.IsDeleted,
            iv,
            blob) { User = user };
    }

    private EncryptedBlobDto MapEncryptedBlobDto(EncryptedBlob encryptedBlob)
    {
        var iv = Convert.ToBase64String(encryptedBlob.InitializationVector);
        var blob = Convert.ToBase64String(encryptedBlob.Blob);

        return new EncryptedBlobDto(
            encryptedBlob.Id,
            encryptedBlob.UpdatedAt,
            encryptedBlob.IsDeleted,
            iv,
            blob);
    }
}