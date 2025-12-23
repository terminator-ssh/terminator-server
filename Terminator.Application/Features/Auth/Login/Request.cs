using MediatR;
using Terminator.Core.Result;

namespace Terminator.Application.Features.Auth.Login;

public record Request(
    string Username,
    string LoginKey) : IRequest<Result<Response>>;