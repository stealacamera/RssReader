namespace RssReader.Application.Common.DTOs;

public record Tag(int Id, string Name);
public record FeedSubscriptionTag(int FeedSubscriptionId, int TagId);