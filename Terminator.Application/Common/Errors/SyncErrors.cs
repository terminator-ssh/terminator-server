using Terminator.Core.Result;

namespace Terminator.Application.Common.Errors;

public static partial class ValidationErrors
{
    public static partial class Sync
    {
        public static partial class EncryptedBlob
        {
            public static readonly Error IdRequired = new(
                "Sync.EncryptedBlob.Id.Required", 
                "The id is required.");
            
            public static readonly Error BlobRequired = new(
                "Sync.EncryptedBlob.Blob.Required", 
                "The blob data is required.");

            public static readonly Error BlobInvalidFormat = new(
                "Sync.EncryptedBlob.Blob.InvalidFormat",
                "The blob data must be a valid Base64 string.");
            
            public static readonly Error IvRequired = new(
                "Sync.EncryptedBlob.Iv.Required", 
                "The initialization vector data is required.");

            public static readonly Error IvInvalidFormat = new(
                "Sync.EncryptedBlob.Iv.InvalidFormat",
                "The initialization vector data must be a valid Base64 string.");
        }
    }
}