using MediatR;
using Terminator.Core.Result;

namespace Terminator.Application.Features.Sync;

public record Request(
    IReadOnlyList<EncryptedBlobDto> Blobs,
    DateTimeOffset LastSyncTime) : IRequest<Result<Response>>;