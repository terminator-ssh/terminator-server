namespace Terminator.Application.Features.User.ListAll;

public record Response(IEnumerable<UserDto> Users);