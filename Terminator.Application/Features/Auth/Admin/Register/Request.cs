using MediatR;
using Terminator.Core.Result;

namespace Terminator.Application.Features.Auth.Admin.Register;

public record Request(
    string Username,
    string Password) : IRequest<Result<Response>>;