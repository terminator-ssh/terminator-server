using Terminator.Core.Result;

namespace Terminator.Application.Common.Errors;

public static partial class ValidationErrors
{
    public static partial class User
    {
        public static readonly Error UserIdRequired = new(
            "User.Id.Required",
            "User ID is required.");
    }
}