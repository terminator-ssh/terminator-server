using Terminator.Core.Common;

namespace Terminator.Core.Entities;

public class User : BaseEntity<Guid>
{
    public User(
        Guid id, 
        string username, 
        byte[] keySalt, 
        byte[] authSalt, 
        byte[] encryptedMasterKey, 
        byte[] loginHash) : base(id)
    {
        Username = username;
        KeySalt = keySalt;
        AuthSalt = authSalt;
        EncryptedMasterKey = encryptedMasterKey;
        LoginHash = loginHash;
    }

    public string Username { get; set; }
    public byte[] KeySalt { get; set; }
    public byte[] AuthSalt { get; set; }
    public byte[] EncryptedMasterKey { get; set; }
    public byte[] LoginHash { get; set; }
    public IList<EncryptedBlob> Blobs { get; set; } = new List<EncryptedBlob>();
}