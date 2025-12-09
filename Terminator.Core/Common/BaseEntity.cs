namespace Terminator.Core.Common;

public class BaseEntity<TKey>
{
    public BaseEntity(TKey id)
    {
        Id = id;
    }
    
    public TKey Id { get; set; }
}