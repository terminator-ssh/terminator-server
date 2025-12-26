namespace Terminator.Application.Features.Auth.Preflight;

public record Response(
    string AuthSalt,
    string KeySalt,
    string EncryptedMasterKey);