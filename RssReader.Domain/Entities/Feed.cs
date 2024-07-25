using RssReader.Domain.Common;

namespace RssReader.Domain.Entities;

public class Feed : BaseEntity
{
    public string Url { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int FolderId { get; set; }

    public DateTime CreatedAt { get; set; }
}
