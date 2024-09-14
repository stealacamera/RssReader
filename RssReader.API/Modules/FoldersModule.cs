using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using RssReader.API.Common.DTOs.Requests;
using RssReader.Application.Behaviour.Operations.Feeds.Queries.GetAllForFolder;
using RssReader.Application.Behaviour.Operations.Folders.Commands.Create;
using RssReader.Application.Behaviour.Operations.Folders.Queries.GetChildrenFolders;
using RssReader.Application.Common.DTOs;

namespace RssReader.API.Modules;

public class FoldersModule : CarterModule
{
    public FoldersModule() : base("/folders")
    {
        RequireAuthorization();
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("", Create);
        app.MapGet("{id}/children", GetSubFolders).WithDescription("Gets subfolders");
        app.MapGet("{id}/feeds", GetFeeds);
    }

    private async Task<Ok<IList<Folder>>> GetSubFolders(int id, ISender sender)
    {
        var subFolders = await sender.Send(new GetChildrenFoldersQuery(1, id));
        return TypedResults.Ok(subFolders);
    }

    private async Task<Created<Folder>> Create(CreateFolderRequest request, ISender sender)
    {
        var command = new CreateFolderCommand(1, request.Name, request.ParentFolderId);
        var newFolder = await sender.Send(command);

        return TypedResults.Created(string.Empty, newFolder);
    }

    private async Task<Ok<IList<Feed>>> GetFeeds(int id, ISender sender)
    {
        var command = new GetAllFeedsForFolderQuery(id, 1);
        var feeds = await sender.Send(command);

        return TypedResults.Ok(feeds);
    }
}
