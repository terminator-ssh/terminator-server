using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Terminator.Application.Common;
using Terminator.Core.Common.Errors;
using Terminator.Core.Entities;
using Terminator.Core.Result;

namespace Terminator.Application.Features.User.GetActiveBlobCount;

public class Handler(
    IApplicationDbContext db, 
    ILogger<Handler> logger) : IRequestHandler<Request, Result<Response>>
{
    public async Task<Result<Response>> Handle(Request request, CancellationToken ct)
    {
        var user = await db.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, ct);
        if (user is null)
            return Result<Response>.Error(ErrorType.NotFound, DomainErrors.User.NotFound);

        int activeBlobCount = await db.EncryptedBlobs
            .Where(x => x.User.Id == request.UserId)
            .CountAsync(EncryptedBlob.ActiveBlobFilter, ct);
        
        return Result<Response>.Success(new(activeBlobCount));
    }
}