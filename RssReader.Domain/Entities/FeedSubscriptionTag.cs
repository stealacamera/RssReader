using RssReader.Domain.Common;

namespace RssReader.Domain.Entities;

public class FeedSubscriptionTag : BaseEntity
{
    public int TagId { get; set; }
    public int FeedSubscriptionId { get; set; }
}
