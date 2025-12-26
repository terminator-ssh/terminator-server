namespace Terminator.Web.DTOs.Auth.Admin;

public record AdminRegisterRequestDto(
    string Username,
    string Password);