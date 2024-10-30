using RssReader.Domain.Common;

namespace RssReader.Domain.Entities;

public class FeedSubscription : BaseSimpleEntity<int>
{
    public string Name { get; set; } = null!;

    public int FeedId { get; set; }

    public int FolderId { get; set; }
    public Folder Folder { get; set; }
}
