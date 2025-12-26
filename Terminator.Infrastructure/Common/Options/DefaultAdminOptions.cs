using System.ComponentModel.DataAnnotations;

namespace Terminator.Infrastructure.Common.Options;

public class DefaultAdminOptions
{
    public const string SectionName = "DefaultAdmin";
    
    public string? Username { get; set; }
    public string? Password { get; set; }
}