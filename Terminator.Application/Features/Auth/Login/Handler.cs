using System.Security.Cryptography;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Terminator.Application.Common;
using Terminator.Core.Common.Errors;
using Terminator.Core.Result;

namespace Terminator.Application.Features.Auth.Login;

public class LoginHandler(
    IApplicationDbContext db, 
    IJwtProvider jwtProvider,
    ILogger<LoginHandler> logger) 
    : IRequestHandler<Request, Result<Response>>
{
    public async Task<Result<Response>> Handle(Request request, CancellationToken ct)
    {
        var user = await db.Users
            .FirstOrDefaultAsync(x => x.Username == request.Username, ct);

        logger.LogDebug(
            "Login request for username: {username}, login key: {loginKey}. Found user: {foundUser}",
            request.Username, request.LoginKey, user is not null);
        
        if (user is null)
            return Result<Response>.Error(ErrorType.Validation, DomainErrors.User.InvalidCredentials);

        var loginKeyBytes = Convert.FromBase64String(request.LoginKey);
        var computedHash = SHA256.HashData(loginKeyBytes);

        logger.LogDebug(
            "Login attempt for user '{username}'. " +
            "DB hash (base64): {dbHash}, request hash (base64): {requestHash}",
            request.Username, Convert.ToBase64String(user.LoginHash), Convert.ToBase64String(computedHash));
        
        if (!computedHash.SequenceEqual(user.LoginHash))
            return Result<Response>.Error(ErrorType.Validation, DomainErrors.User.InvalidCredentials);

        var token = jwtProvider.Generate(user);
        
        logger.LogDebug("Login attempt for user '{username}' successful. JWT: {token}",
            request.Username, token);
        
        return Result<Response>.Success(new Response(token));
    }
}