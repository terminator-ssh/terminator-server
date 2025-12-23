using Terminator.Core.Common;

namespace Terminator.Core.Entities;

public class EncryptedBlob : BaseEntity<Guid>
{
    public EncryptedBlob(
        Guid id,
        DateTimeOffset updatedAt,
        bool isDeleted,
        byte[] initializationVector, 
        byte[] blob) : base(id)
    {
        UpdatedAt = updatedAt;
        IsDeleted = isDeleted;
        InitializationVector = initializationVector;
        Blob = blob;
    }

    public DateTimeOffset UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public byte[] InitializationVector { get; set; }
    public byte[] Blob { get; set; }
    public required User User { get; set; }
}