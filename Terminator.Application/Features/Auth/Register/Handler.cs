using System.Security.Cryptography;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Terminator.Application.Common;
using Terminator.Core.Common.Errors;
using Terminator.Core.Entities;
using Terminator.Core.Result;

namespace Terminator.Application.Features.Auth.Register;

public class Handler(
    IApplicationDbContext db, 
    IJwtProvider jwtProvider,
    ILogger<Handler> logger) 
    : IRequestHandler<Request, Result<Response>>
{
    public async Task<Result<Response>> Handle(Request request, CancellationToken ct)
    {
        var username = request.Username;

        if (await db.Users.FirstOrDefaultAsync(x => x.Username == username, ct) is not null)
        {
            return Result<Response>.Error(ErrorType.Validation, DomainErrors.User.AlreadyExists);
        }

        var loginKeyBytes = Convert.FromBase64String(request.LoginKey);
        var loginHash = SHA256.HashData(loginKeyBytes);

        var user = new User(
            Guid.NewGuid(),
            request.Username,
            Convert.FromBase64String(request.KeySalt),
            Convert.FromBase64String(request.AuthSalt),
            Convert.FromBase64String(request.EncryptedMasterKey),
            loginHash
        );
        
        db.Users.Add(user);
        await db.SaveChangesAsync(ct);

        var token = jwtProvider.Generate(user);
        
        logger.LogDebug(
            "Registered user {username}. " +
            "Requested auth salt: {requestAuthSalt}, " +
            "requested key salt: {requestKeySalt}. " +
            "Actual auth salt: {authSalt}, " +
            "actual key salt: {keySalt}. " +
            "Generated JWT: {jwt}",
            user.Username, 
            request.AuthSalt, 
            request.KeySalt, 
            Convert.ToBase64String(user.AuthSalt), 
            Convert.ToBase64String(user.KeySalt),
            token);

        return Result<Response>.Success(new Response(token));
    }
}