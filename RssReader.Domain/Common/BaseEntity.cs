namespace RssReader.Domain.Common;

public abstract class BaseEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public abstract class BaseSimpleEntity<TKey> : BaseEntity 
    where TKey : struct, IComparable<TKey>
{
    public TKey Id { get; set; }
}