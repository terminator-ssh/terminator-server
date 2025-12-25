using Terminator.Core.Common;
using Terminator.Core.Result;

namespace Terminator.Application.Common.Errors;

public static partial class ValidationErrors
{
    public static partial class Auth
    {
        public static readonly Error UsernameRequired = new(
            "Auth.Username.Required", 
            "The username is required.");
        public static readonly Error UsernameTooLong = new(
            "Auth.Username.TooLong", 
            $"Username must not exceed {UserConstants.MaxUsernameLength} characters.");
        
        public static readonly Error AuthSaltRequired = new(
            "Auth.AuthSalt.Required", 
            "Auth Salt is required.");
        public static readonly Error AuthSaltInvalid = new(
            "Auth.AuthSalt.InvalidFormat", 
            "Auth Salt must be a valid Base64 string.");
        
        public static readonly Error KeySaltRequired = new(
            "Auth.KeySalt.Required", 
            "Key Salt is required.");
        public static readonly Error KeySaltInvalid = new(
            "Auth.KeySalt.InvalidFormat", 
            "Key Salt must be a valid Base64 string.");
        
        public static readonly Error LoginKeyRequired = new(
            "Auth.LoginKey.Required", 
            "Login Key is required.");
        public static readonly Error LoginKeyInvalid = new(
            "Auth.LoginKey.InvalidFormat", 
            "Login Key must be a valid Base64 string.");
        
        public static readonly Error MasterKeyRequired = new(
            "Auth.MasterKey.Required", 
            "Master Key is required.");
        public static readonly Error MasterKeyInvalid = new(
            "Auth.MasterKey.InvalidFormat", 
            "Encrypted Master Key must be a valid Base64 string.");
    }
}