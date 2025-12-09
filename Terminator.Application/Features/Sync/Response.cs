namespace Terminator.Application.Features.Sync;

public record Response(
    IEnumerable<EncryptedBlobDto> Blobs,
    DateTimeOffset SyncTime);