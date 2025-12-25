namespace Terminator.Core.Common;

public class RoleType
{
    private readonly string _roleType;

    private RoleType(string roleType)
    {
        _roleType = roleType;
    }
    
    public const string UserRole = "user";
    public static readonly RoleType User = new(UserRole);
    
    public const string AdminRole = "admin";
    public static readonly RoleType Admin = new(AdminRole);

    public override string ToString()
    {
        return _roleType;
    }
}