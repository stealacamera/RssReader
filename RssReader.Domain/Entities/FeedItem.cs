using RssReader.Domain.Common;

namespace RssReader.Domain.Entities;

public class FeedItem : BaseSimpleEntity<int>
{
    public int FeedId { get; set; }
    public Feed Feed { get; set; } = null!;

    public string? ItemId { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Link { get; set; }
    public string? Description { get; set; }
    public string? Content { get; set; }
    public DateTime? PublishedAt { get; set; }

}
