namespace RssReader.API.Common.DTOs.Folder;

public record CreateFolderRequest(string Name, int? ParentFolderId = null);