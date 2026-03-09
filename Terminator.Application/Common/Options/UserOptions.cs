using Terminator.Core.Entities;

namespace Terminator.Application.Common.Options;

public class UserOptions
{
    public const string SectionName = "UserSettings";
    
    public UserAccountType DefaultAccountType { get; set; }
    public int BlobLimit { get; set; }
}