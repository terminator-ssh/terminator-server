using Terminator.Core.Entities;

namespace Terminator.Application.Features.Sync;

public record EncryptedBlobDto(
    Guid Id,
    DateTimeOffset UpdatedAt,
    bool IsDeleted,
    string Iv,
    string Blob);