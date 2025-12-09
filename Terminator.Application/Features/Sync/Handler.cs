using MediatR;
using Microsoft.EntityFrameworkCore;
using Terminator.Core.Entities;
using Terminator.Core.Result;
using Terminator.Infrastructure.Data;

namespace Terminator.Application.Features.Sync;

public class Handler : IRequestHandler<Request, Result<Response>>
{
    private readonly ApplicationDbContext _db;
    private readonly TimeProvider _timeProvider;

    public Handler(ApplicationDbContext db, TimeProvider timeProvider)
    {
        _db = db;
        _timeProvider = timeProvider;
    }
    
    public async Task<Result<Response>> Handle(Request request, CancellationToken cancellationToken)
    {
        var clientBlobIds = request.Blobs.Select(x => x.Id).ToList();
        var clientBlobsById = 
            request.Blobs
                .DistinctBy(x => x.Id) 
                .ToDictionary(x => x.Id);
        
        var serverBlobTimestampsById = await _db.EncryptedBlobs
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
                var encryptedBlob = MapEncryptedBlob(clientBlob);
                blobsToAdd.Add(encryptedBlob);
            }

            else if (clientBlob.UpdatedAt > serverBlobTimestamp)
            {
                var encryptedBlob = MapEncryptedBlob(clientBlob);
                blobsToUpdate.Add(encryptedBlob);
            }
            
            else if (clientBlob.UpdatedAt < serverBlobTimestamp)
            {
                clientStaleIds.Add(clientBlob.Id);
            }
        }
        
        _db.EncryptedBlobs.AddRange(blobsToAdd);
        _db.EncryptedBlobs.UpdateRange(blobsToUpdate);

        await _db.SaveChangesAsync(cancellationToken);

        var updatedOrAddedBlobIds = new List<Guid>();
        updatedOrAddedBlobIds.AddRange(blobsToUpdate.Select(x => x.Id));
        updatedOrAddedBlobIds.AddRange(blobsToAdd.Select(x => x.Id));
        
        var newBlobs = await _db.EncryptedBlobs
            .Where(x
                => (x.UpdatedAt > request.LastSyncTime 
                   && !updatedOrAddedBlobIds.Contains(x.Id)) 
                   || clientStaleIds.Contains(x.Id))
            .ToListAsync(cancellationToken);
        
        var newBlobDtos = newBlobs.Select(MapEncryptedBlobDto);

        var response = new Response(newBlobDtos, _timeProvider.GetUtcNow());

        return Result<Response>.Success(response);
    }

    // TODO: Mappers
    private EncryptedBlob MapEncryptedBlob(EncryptedBlobDto dto)
    {
        var iv = Convert.FromBase64String(dto.Iv);
        var blob = Convert.FromBase64String(dto.Blob);

        return new EncryptedBlob(
            dto.Id,
            dto.UpdatedAt,
            dto.IsDeleted,
            iv,
            blob);
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