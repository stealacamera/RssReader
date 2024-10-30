using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using RssReader.API.Common;
using RssReader.API.Common.Authorization;
using RssReader.API.Common.DTOs.Requests;
using RssReader.Application.Behaviour.Operations.FeedItems.Queries.GetAllForFolder;
using RssReader.Application.Behaviour.Operations.FeedSubscriptions.Create;
using RssReader.Application.Behaviour.Operations.Folders.Commands.Create;
using RssReader.Application.Behaviour.Operations.Folders.Commands.Delete;
using RssReader.Application.Behaviour.Operations.Folders.Queries.GetAllForUser;
using RssReader.Application.Behaviour.Operations.Folders.Queries.GetById;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Enums;

namespace RssReader.API.Modules;

public class FoldersModule : BaseCarterModule
{
    public FoldersModule() : base("/folders")
    {
        RequireAuthorization();
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("", CreateAsync)
           .Produces(StatusCodes.Status401Unauthorized)
           .Produces(StatusCodes.Status404NotFound)
           .RequireAuthorization(new HasPermissionAttribute(Permissions.CrudFolders));

        app.MapGet("", GetAllFoldersForUserAsync)
           .RequireAuthorization(new HasPermissionAttribute(Permissions.CrudFolders));

        app.MapGet("{id}", GetFolderAsync)
           .Produces(StatusCodes.Status401Unauthorized)
           .Produces(StatusCodes.Status404NotFound)
           .RequireAuthorization(new HasPermissionAttribute(Permissions.CrudFolders));

        app.MapPost("{id}/feeds", AddFeedAsync)
            .RequireAuthorization(new HasPermissionAttribute(Permissions.CrudFolders));

        app.MapGet("{id}/feedItems", GetFeedItemsForFolderAsync)
           .WithSummary("Gets all feed items for the given folder and its subfolders")
           .Produces(StatusCodes.Status401Unauthorized)
           .Produces(StatusCodes.Status404NotFound)
           .RequireAuthorization(new HasPermissionAttribute(Permissions.CrudFolders));

        app.MapDelete("{id}", DeleteFolderAsync)
           .Produces(StatusCodes.Status401Unauthorized)
           .Produces(StatusCodes.Status404NotFound)
           .RequireAuthorization(new HasPermissionAttribute(Permissions.CrudFolders));
    }

    private async Task<Ok<IList<SimpleFolder>>> GetAllFoldersForUserAsync(
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var query = new GetAllFoldersForUserQuery(GetRequesterId(httpContext));
        var folders = await sender.Send(query, cancellationToken);

        return TypedResults.Ok(folders);
    }

    private async Task<Created<FeedSubscription>> AddFeedAsync(
        int id,
        SubscribeToFeedRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var command = new SubscribeToFeedCommand(GetRequesterId(httpContext), id, request.FeedUrl, request.FeedName);
        var subscription = await sender.Send(command, cancellationToken);

        return TypedResults.Created(string.Empty, subscription);
    }

    private async Task<Created<Folder>> CreateAsync(
        CreateFolderRequest request, 
        ISender sender, 
        HttpContext httpContext, 
        CancellationToken cancellationToken)
    {
        var command = new CreateFolderCommand(
            GetRequesterId(httpContext), 
            request.Name, 
            request.ParentFolderId);
        
        var newFolder = await sender.Send(command, cancellationToken);
        return TypedResults.Created(string.Empty, newFolder);
    }

    private async Task<Ok<Folder>> GetFolderAsync(
        int id, 
        ISender sender, 
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var query = new GetFolderByIdQuery(GetRequesterId(httpContext), id);
        var folder = await sender.Send(query, cancellationToken);

        return TypedResults.Ok(folder);
    }

    private async Task<Ok<PaginatedResponse<DateTime, IList<FeedItem>>>> GetFeedItemsForFolderAsync(
        int id, 
        int pageSize,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken,
        DateTime? cursor)
    {
        var request = new GetAllFeedItemsForFolderQuery(GetRequesterId(httpContext), id, pageSize, cursor);
        var feedItems = await sender.Send(request, cancellationToken);

        return TypedResults.Ok(feedItems);
    }

    private async Task<NoContent> DeleteFolderAsync(
        int id, 
        ISender sender, 
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var request = new DeleteFolderCommand(GetRequesterId(httpContext), id);
        await sender.Send(request, cancellationToken);

        return TypedResults.NoContent();
    }
}
