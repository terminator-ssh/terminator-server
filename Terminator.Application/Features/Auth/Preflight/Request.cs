using MediatR;
using Terminator.Core.Result;

namespace Terminator.Application.Features.Auth.Preflight;

public record Request(string Username) : IRequest<Result<Response>>;