using System.Diagnostics.Metrics;
using Terminator.Application.Common;

namespace Terminator.Web.Services;

public class MetricsCollector
{
    private readonly IServiceScopeFactory _scopeFactory;

    public const string MeterName = "Terminator";
    
    public MetricsCollector(
        IMeterFactory meterFactory, 
        IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
        var meter = meterFactory.Create(MeterName);

        meter.CreateObservableGauge(
            "terminator_users_total", 
            GetTotalUsers, 
            description: "Total registered users");

        meter.CreateObservableGauge(
            "terminator_users_active_total", 
            GetActiveUsers, 
            description: "Active users in a time window");
    }

    private int GetTotalUsers()
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        
        return db.Users.Count();
    }

    private IEnumerable<Measurement<int>> GetActiveUsers()
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var time = scope.ServiceProvider.GetRequiredService<TimeProvider>();
        
        var now = time.GetUtcNow();

        var active1d = db.UserMetrics.Count(x => x.LastSyncAt >= now.AddDays(-1));
        var active7d = db.UserMetrics.Count(x => x.LastSyncAt >= now.AddDays(-7));
        var active30d = db.UserMetrics.Count(x => x.LastSyncAt >= now.AddDays(-30));

        return
        [
            new Measurement<int>(active1d, new KeyValuePair<string, object?>("window", "1d")),
            new Measurement<int>(active7d, new KeyValuePair<string, object?>("window", "7d")),
            new Measurement<int>(active30d, new KeyValuePair<string, object?>("window", "30d"))
        ];
    }
}