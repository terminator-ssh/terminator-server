using Terminator.Core.Entities;

namespace Terminator.Application.Common;

public interface IJwtProvider
{
    string Generate(User user);
}