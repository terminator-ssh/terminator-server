using System.ComponentModel;

namespace Terminator.Application.Common.Options;

public class MetricsOptions
{
    public const string SectionName = "MetricsSettings";
    
    [DefaultValue(false)]
    public bool Enabled { get; set; }
}