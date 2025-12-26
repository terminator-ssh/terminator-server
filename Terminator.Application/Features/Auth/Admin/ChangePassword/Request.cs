using MediatR;
using Terminator.Core.Result;

namespace Terminator.Application.Features.Auth.Admin.ChangePassword;

public record Request(
    Guid Id,
    string NewPassword) : IRequest<Result<Response>>;