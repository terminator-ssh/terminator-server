using Terminator.Application.Features.Sync;

namespace Terminator.Web.DTOs.Sync;

public record SyncRequestDto(
    IReadOnlyList<EncryptedBlobDto> Blobs,
    DateTimeOffset LastSyncTime);