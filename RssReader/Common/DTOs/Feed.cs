namespace RssReader.API.Common.DTOs;

public record CreateFeedRequest(int FolderId, string Url, string? Name = null);