using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Terminator.Application.Common;
using Terminator.Core.Result;
using Terminator.Core.Entities;
using Entities = Terminator.Core.Entities;

namespace Terminator.Application.Features.User.ListAll;

public class Handler(
    IApplicationDbContext db) : IRequestHandler<Request, Result<Response>>
{
    public async Task<Result<Response>> Handle(Request request, CancellationToken ct)
    {
        var users = await db.Users.ToListAsync(ct);
        var userDtos = users.Select(MapUserDto).ToList();
        return Result<Response>.Success(new Response(userDtos));
    }
    
    // TODO mappers
    private UserDto MapUserDto(Entities.User user)
    {
        return new UserDto(user.Id, user.Username);
    }
}