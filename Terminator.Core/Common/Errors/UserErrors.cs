using Terminator.Core.Result;

namespace Terminator.Core.Common.Errors;

public static partial class DomainErrors
{
    public static class User
    {
        public static readonly Error AlreadyExists = new(
            "User.AlreadyExists", "Username is already taken.");
        
        public static readonly Error NotFound = new(
            "User.NotFound", "User not found.");
        
        public static readonly Error InvalidCredentials = new(
            "User.InvalidCredentials", "Invalid credentials.");
    }
}