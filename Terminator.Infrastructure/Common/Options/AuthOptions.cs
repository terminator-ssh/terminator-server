using System.ComponentModel.DataAnnotations;

namespace Terminator.Infrastructure.Common.Options;

public class AuthOptions
{
    public const string SectionName = "AuthSettings";
    
    [Base64String]
    public string? SecretKey { get; set; }

    [Required]
    [MinLength(1)]
    public string Issuer { get; set; } = string.Empty;
    
    [Required]
    [MinLength(1)]
    public string Audience { get; set; } = string.Empty;
    
    [Required]
    [Range(1, int.MaxValue)]
    public int ExpirationDays { get; set; }
}