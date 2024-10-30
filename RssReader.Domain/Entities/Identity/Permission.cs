using RssReader.Domain.Common;

namespace RssReader.Domain.Entities.Identity;

public class Permission : BaseSimpleEntity<int>
{
    public string Name { get; set; } = null!;
}
