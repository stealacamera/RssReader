namespace RssReader.Application.Common.DTOs;

public record Folder(int Id, string Name, int OwnerId);

public record CreateFolderRequest(string Name, int? ParentFolderId = null);