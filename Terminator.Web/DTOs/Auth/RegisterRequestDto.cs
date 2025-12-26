namespace Terminator.Web.DTOs.Auth;

public record RegisterRequestDto(
    string Username,
    string AuthSalt,
    string KeySalt,
    string EncryptedMasterKey,
    string LoginKey);