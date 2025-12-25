using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Terminator.Application.Common;
using Terminator.Core.Common.Errors;
using Terminator.Core.Result;

namespace Terminator.Application.Features.Auth.Admin.Login;

public class Handler(
    IApplicationDbContext db,
    ILogger<Handler> logger,
    IJwtProvider jwtProvider,
    IPasswordHasher<Core.Entities.Admin> hasher) : IRequestHandler<Request, Result<Response>>
{
    public async Task<Result<Response>> Handle(Request request, CancellationToken ct)
    {
        var admin = await db.Admins.FirstOrDefaultAsync(x => x.Username == request.Username, ct);
        if (admin is null)
            return Result<Response>.Error(ErrorType.NotFound, DomainErrors.Admin.NotFound);
        
        var verifyResult = hasher.VerifyHashedPassword(admin, admin.PasswordHash, request.Password);
        if(verifyResult == PasswordVerificationResult.Failed)
            return Result<Response>.Error(ErrorType.Unauthorized, DomainErrors.Admin.InvalidCredentials);

        var token = jwtProvider.Generate(admin);

        logger.LogDebug("Login attempt for admin {username} successful. JWT: {token}",
            admin.Username, token);

        return Result<Response>.Success(new(token));
    }
}