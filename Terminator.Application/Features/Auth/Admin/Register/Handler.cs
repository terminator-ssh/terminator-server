using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Terminator.Application.Common;
using Terminator.Core.Common.Errors;
using Terminator.Core.Result;

namespace Terminator.Application.Features.Auth.Admin.Register;

public class Handler(
    IApplicationDbContext db,
    ILogger<Handler> logger,
    IJwtProvider jwtProvider,
    IPasswordHasher<Core.Entities.Admin> hasher) : IRequestHandler<Request, Result<Response>>
{
    public async Task<Result<Response>> Handle(Request request, CancellationToken ct)
    {
        var existingAdmin = await db.Admins.FirstOrDefaultAsync(x => x.Username == request.Username, ct);
        if (existingAdmin is not null)
            return Result<Response>.Error(ErrorType.Validation, DomainErrors.Admin.AlreadyExists);

        var admin = new Core.Entities.Admin(Guid.NewGuid(), request.Username, string.Empty);
        
        var passwordHash = hasher.HashPassword(admin, request.Password);
        admin.PasswordHash = passwordHash;

        db.Admins.Add(admin);
        await db.SaveChangesAsync(ct);

        var token = jwtProvider.Generate(admin);

        logger.LogDebug("Login attempt for admin {username} successful. JWT: {token}",
            admin.Username, token);

        return Result<Response>.Success(new(token));
    }
}