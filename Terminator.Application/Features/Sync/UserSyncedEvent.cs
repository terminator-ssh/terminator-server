using MediatR;

namespace Terminator.Application.Features.Sync;

public record UserSyncedEvent(
    Guid UserId, 
    DateTimeOffset Timestamp) : INotification;