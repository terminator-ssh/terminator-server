using Terminator.Core.Common;

namespace Terminator.Core.Entities;

public class Admin : BaseEntity<Guid>
{
    public Admin(
        Guid id, 
        string username,
        string passwordHash) : base(id)
    {
        Username = username;
        PasswordHash = passwordHash;
    }

    public string Username { get; set; }
    public string PasswordHash { get; set;  }
}