namespace Terminator.Application.Telemetry;

public class UserMetric(
    Guid userId, 
    DateTimeOffset firstSyncAt, 
    DateTimeOffset lastSyncAt, 
    long syncCount)
{
    public Guid UserId { get; set; } = userId;
    public DateTimeOffset FirstSyncAt { get; set; } = firstSyncAt;
    public DateTimeOffset LastSyncAt { get; set; } = lastSyncAt;
    public long SyncCount { get; set; } = syncCount;
}