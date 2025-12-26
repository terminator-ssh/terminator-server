using Terminator.Core.Common;
using Terminator.Core.Result;

namespace Terminator.Application.Common.Errors;

public static partial class ValidationErrors
{
    public static partial class Auth
    {
        public partial class Admin
        {
            public static readonly Error UsernameRequired = new(
                "Auth.Admin.Username.Required", 
                "The username is required.");
            public static readonly Error UsernameTooLong = new(
                "Auth.Admin.Username.TooLong", 
                $"Username must not exceed {AdminConstants.MaxUsernameLength} characters.");
            
            public static readonly Error PasswordRequired = new(
                "Auth.Admin.Password.Required", 
                "The password is required.");
            public static readonly Error PasswordTooLong = new(
                "Auth.Admin.Password.TooLong", 
                $"Password must not exceed {AdminConstants.MaxPasswordLength} characters.");
            
            public static readonly Error IdRequired = new(
                "Auth.Admin.Id.Required", 
                "The id is required.");
        }
    }
}