using System.Security.Cryptography;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Terminator.Application.Common;
using Terminator.Core.Common.Errors;
using Terminator.Core.Result;

namespace Terminator.Application.Features.Auth.Login;

public class LoginHandler(
    IApplicationDbContext db, 
    IJwtProvider jwtProvider) 
    : IRequestHandler<Request, Result<Response>>
{
    public async Task<Result<Response>> Handle(Request request, CancellationToken ct)
    {
        var user = await db.Users
            .FirstOrDefaultAsync(x => x.Username == request.Username, ct);

        if (user is null)
            return Result<Response>.Error(ErrorType.Validation, DomainErrors.User.InvalidCredentials);

        var loginKeyBytes = Convert.FromBase64String(request.LoginKey);
        var computedHash = SHA256.HashData(loginKeyBytes);

        if (!computedHash.SequenceEqual(user.LoginHash))
            return Result<Response>.Error(ErrorType.Validation, DomainErrors.User.InvalidCredentials);

        var token = jwtProvider.Generate(user);
        return Result<Response>.Success(new Response(token));
    }
}