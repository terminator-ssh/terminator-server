using MediatR;
using Terminator.Core.Result;

namespace Terminator.Application.Features.User.ListAll;

public record Request() : IRequest<Result<Response>>;