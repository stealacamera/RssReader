namespace RssReader.Application.Common.DTOs;

public record SimpleFolder(int Id, string Name);

public record Folder(
    int Id, 
    string Name, 
    IList<SimpleFolder>? Subfolders = null, 
    IList<FeedSubscription>? Feeds = null) 
    : SimpleFolder(Id, Name);