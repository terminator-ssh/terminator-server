using MediatR;
using Microsoft.EntityFrameworkCore;
using Terminator.Application.Common;
using Terminator.Core.Common.Errors;
using Terminator.Core.Result;

namespace Terminator.Application.Features.Auth.Preflight;

public class Handler(IApplicationDbContext db) 
    : IRequestHandler<Request, Result<Response>>
{
    public async Task<Result<Response>> Handle(Request request, CancellationToken ct)
    {
        var user = await db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Username == request.Username, ct);

        if (user is null)
            return Result<Response>.Error(ErrorType.Validation, DomainErrors.User.NotFound);

        return Result<Response>.Success(new Response(
            Convert.ToBase64String(user.AuthSalt),
            Convert.ToBase64String(user.KeySalt),
            Convert.ToBase64String(user.EncryptedMasterKey)
        ));
    }
}