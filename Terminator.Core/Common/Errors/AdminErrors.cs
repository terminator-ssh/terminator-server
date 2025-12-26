using Terminator.Core.Result;

namespace Terminator.Core.Common.Errors;

public static partial class DomainErrors
{
    public static class Admin
    {
        public static readonly Error AlreadyExists = new(
            "Admin.AlreadyExists", "Username is already taken.");
        
        public static readonly Error NotFound = new(
            "Admin.NotFound", "Admin not found.");
        
        public static readonly Error InvalidCredentials = new(
            "Admin.InvalidCredentials", "Invalid credentials.");
    }
}