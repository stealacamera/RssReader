namespace RssReader.Application.Common.DTOs;

public record Feed(int Id, string Url, string Name, string? IconUrl);
public record FeedSubscription(int FeedSubscriptionId, string SubscriptionName, IList<Tag>? Tags = null);