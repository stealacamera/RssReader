namespace RssReader.Domain.Common;

public abstract class BaseEntity 
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public abstract class BaseSimpleEntity : BaseEntity
{
    public int Id { get; set; }
}