using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Terminator.Application.Common;
using Terminator.Core.Common.Errors;
using Terminator.Core.Result;

namespace Terminator.Application.Features.Auth.Admin.ChangePassword;

public class Handler(
    IApplicationDbContext db,
    ILogger<Handler> logger,
    IJwtProvider jwtProvider,
    IPasswordHasher<Core.Entities.Admin> hasher) : IRequestHandler<Request, Result<Response>>
{
    public async Task<Result<Response>> Handle(Request request, CancellationToken ct)
    {
        var admin = await db.Admins.FirstOrDefaultAsync(x => x.Id == request.Id, ct);
        if (admin is null)
            return Result<Response>.Error(ErrorType.NotFound, DomainErrors.Admin.NotFound);

        var newPasswordHash = hasher.HashPassword(admin, request.NewPassword);
        admin.PasswordHash = newPasswordHash;
        await db.SaveChangesAsync(ct);
        
        logger.LogDebug("Changed password for admin {username}", admin.Username);

        var token = jwtProvider.Generate(admin);

        return Result<Response>.Success(new(token));
    }
}