using MediatR;
using Terminator.Core.Result;

namespace Terminator.Application.Features.Auth.Register;

public record Request(
    string Username,
    string AuthSalt,
    string KeySalt,
    string EncryptedMasterKey,
    string LoginKey) : IRequest<Result<Response>>;
