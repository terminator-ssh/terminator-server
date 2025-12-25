using Terminator.Core.Common;
using Terminator.Core.Entities;

namespace Terminator.Application.Common;

public interface IJwtProvider
{
    string Generate(Guid userId, string username, string role);
    string Generate(Guid userId, string username, RoleType role);
    string Generate(Admin admin);
    string Generate(User user);
}