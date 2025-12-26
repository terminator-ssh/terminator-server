namespace Terminator.Web.DTOs.Auth;

public record LoginRequestDto(
    string Username,
    string LoginKey);