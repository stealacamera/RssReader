using RssReader.Domain.Common;

namespace RssReader.Domain.Entities;

public class Feed : BaseSimpleEntity<int>
{
    public string Url { get; set; } = null!;
    public string? IconUrl { get; set; }
    public string Name { get; set; } = null!;

    public string? Update_ETag { get; set; }
    public string? Update_LastModified { get; set; }
}
