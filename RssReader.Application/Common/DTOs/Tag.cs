namespace RssReader.Application.Common.DTOs;

public record Tag(int Id, string Name);
public record FeedTag(int FeedId, int TagId);