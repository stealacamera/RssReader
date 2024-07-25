using RssReader.Domain.Common;

namespace RssReader.Domain.Entities;

public class FeedTag : BaseEntity
{
    public int TagId { get; set; }
    public int FeedId { get; set; }
}
