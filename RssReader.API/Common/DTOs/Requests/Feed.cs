namespace RssReader.API.Common.DTOs.Requests;

public record CreateFeedRequest(int FolderId, string Url, string Name);
public record SubscribeToFeedRequest(string FeedUrl, string? FeedName = null);