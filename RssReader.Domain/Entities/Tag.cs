using RssReader.Domain.Common;

namespace RssReader.Domain.Entities;

public class Tag : BaseEntity
{
    public string Name { get; set; } = null!;
}
