using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using RssReader.API.Common;
using RssReader.API.Common.DTOs.Requests;
using RssReader.Application.Behaviour.Operations.Feeds.Queries.GetAllForFolder;
using RssReader.Application.Behaviour.Operations.Folders.Commands.Create;
using RssReader.Application.Behaviour.Operations.Folders.Queries.GetChildrenFolders;
using RssReader.Application.Common.DTOs;

namespace RssReader.API.Modules;

public class FoldersModule : BaseCarterModule
{
    public FoldersModule() : base("/folders")
    {
        RequireAuthorization();
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("", Create)
           .Produces(StatusCodes.Status401Unauthorized)
           .Produces(StatusCodes.Status404NotFound);
        
        app.MapGet("{id}/children", GetSubFolders)
           .WithDescription("Gets subfolders")
           .Produces(StatusCodes.Status401Unauthorized)
           .Produces(StatusCodes.Status404NotFound);
        
        app.MapGet("{id}/feeds", GetFeeds)
           .WithDescription("Gets all feeds in a folder")
           .Produces(StatusCodes.Status401Unauthorized)
           .Produces(StatusCodes.Status404NotFound);
    }
    private async Task<Created<Folder>> Create(
        CreateFolderRequest request, 
        ISender sender, 
        HttpContext httpContext)
    {
        var command = new CreateFolderCommand(
            GetRequesterId(httpContext), 
            request.Name, 
            request.ParentFolderId);
        
        var newFolder = await sender.Send(command);
        return TypedResults.Created(string.Empty, newFolder);
    }

    private async Task<Ok<IList<Folder>>> GetSubFolders(int id, ISender sender, HttpContext httpContext)
    {
        var query = new GetChildrenFoldersQuery(GetRequesterId(httpContext), id);
        var subFolders = await sender.Send(query);

        return TypedResults.Ok(subFolders);
    }

    private async Task<Ok<IList<Feed>>> GetFeeds(int id, ISender sender, HttpContext httpContext)
    {
        var command = new GetAllFeedsForFolderQuery(id, GetRequesterId(httpContext));
        var feeds = await sender.Send(command);

        return TypedResults.Ok(feeds);
    }
}
