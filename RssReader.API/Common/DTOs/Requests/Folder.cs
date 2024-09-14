namespace RssReader.API.Common.DTOs.Requests;

public record CreateFolderRequest(string Name, int? ParentFolderId = null);