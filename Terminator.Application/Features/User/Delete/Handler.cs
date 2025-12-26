using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Terminator.Application.Common;
using Terminator.Core.Common.Errors;
using Terminator.Core.Result;

namespace Terminator.Application.Features.User.Delete;

public class Handler(
    IApplicationDbContext db, 
    ILogger<Handler> logger) : IRequestHandler<Request, Result<Response>>
{
    public async Task<Result<Response>> Handle(Request request, CancellationToken ct)
    {
        var user = await db.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, ct);
        if (user is null)
            return Result<Response>.Error(ErrorType.NotFound, DomainErrors.User.NotFound);
           
        db.Users.Remove(user);
        
        await db.SaveChangesAsync(ct);
        
        logger.LogDebug("Removed user {username}", user.Username);
        
        return Result<Response>.Success(new());
    }
}