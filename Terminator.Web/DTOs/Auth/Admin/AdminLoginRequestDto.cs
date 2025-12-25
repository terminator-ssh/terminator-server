namespace Terminator.Web.DTOs.Auth.Admin;

public record AdminLoginRequestDto(
    string Username,
    string Password);