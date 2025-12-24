using System.Security.Cryptography;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Terminator.Application.Common;
using Terminator.Core.Common.Errors;
using Terminator.Core.Entities;
using Terminator.Core.Result;

namespace Terminator.Application.Features.Auth.Register;

public class Handler(
    IApplicationDbContext db, 
    IJwtProvider jwtProvider) 
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

        return Result<Response>.Success(new Response(token));
    }
}