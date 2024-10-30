namespace RssReader.Application.Common.DTOs;

public record FeedItem(
    string FeedName,
    string? FeedIconUrl,

    string? Title,
    string? Author,
    string? Link,
    string? Description,
    string? Content,
    DateTime? PublishedAt);