using Terminator.Core.Result;

namespace Terminator.Core.Common.Errors;

public static partial class DomainErrors
{
    public static partial class Sync
    {
        public static partial class EncryptedBlob
        { 
            public static readonly Error InvalidTimestamp = new(
                "Sync.EncryptedBlob.UpdatedAt.Conflict", 
                "The timestamp is invalid.");
        }
    }
}