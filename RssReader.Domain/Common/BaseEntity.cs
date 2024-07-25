namespace RssReader.Domain.Common;

public abstract class BaseEntity { }

public abstract class BaseSimpleEntity : BaseEntity
{
    public int Id { get; set; }
}