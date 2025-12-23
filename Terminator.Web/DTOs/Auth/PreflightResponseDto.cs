namespace Terminator.Web.DTOs.Auth;

public record PreflightResponseDto(
    string AuthSalt, 
    string KeySalt, 
    string EncryptedMasterKey);