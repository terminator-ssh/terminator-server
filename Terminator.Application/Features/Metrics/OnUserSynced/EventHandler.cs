using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Terminator.Application.Common;
using Terminator.Application.Common.Options;
using Terminator.Application.Features.Sync;
using Terminator.Application.Telemetry;

namespace Terminator.Application.Features.Metrics.OnUserSynced;

public class Handler(
    IApplicationDbContext db,
    IOptions<MetricsOptions> options) 
    : INotificationHandler<UserSyncedEvent>
{
    private readonly MetricsOptions _metricsOptions = options.Value;
    
    public async Task Handle(UserSyncedEvent notification, CancellationToken ct)
    {
        if (!_metricsOptions.Enabled) return;

        var timestamp = notification.Timestamp;

        var metric = await db.UserMetrics
            .FirstOrDefaultAsync(x
                => x.UserId == notification.UserId, ct);

        if (metric is null)
        {
            metric = new UserMetric(
                notification.UserId,
                timestamp,
                timestamp,
                1);
            db.UserMetrics.Add(metric);
        }
        else
        {
            metric.LastSyncAt = timestamp;
            metric.SyncCount++;
        }

        await db.SaveChangesAsync(ct);
    }
}