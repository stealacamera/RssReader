namespace RssReader.Application.Common.DTOs;

public record Tag(int Id, string Name);
public record UpsertTagRequest(string Name);