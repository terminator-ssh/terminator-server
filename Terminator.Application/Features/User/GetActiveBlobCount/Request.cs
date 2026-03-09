using MediatR;
using Terminator.Core.Result;

namespace Terminator.Application.Features.User.GetActiveBlobCount;

public record Request(Guid UserId) : IRequest<Result<Response>>;