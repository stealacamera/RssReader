using RssReader.Domain.Common;

namespace RssReader.Domain.Entities.Identity;

public class Role : BaseSimpleEntity<int>
{
    public string Name { get; set; } = null!;
}