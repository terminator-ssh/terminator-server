using MediatR;
using Terminator.Core.Result;

namespace Terminator.Application.Features.User.Delete;

public record Request(Guid UserId) : IRequest<Result<Response>>;