using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using RssReader.API.Common;
using RssReader.API.Common.Authorization;
using RssReader.API.Common.DTOs.Requests;
using RssReader.Application.Behaviour.Operations.FeedItems.Queries.GetAllForTag;
using RssReader.Application.Behaviour.Operations.Tags.Commands.Create;
using RssReader.Application.Behaviour.Operations.Tags.Commands.Delete;
using RssReader.Application.Behaviour.Operations.Tags.Commands.Edit;
using RssReader.Application.Behaviour.Operations.Tags.Queries.GetAllForUser;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Enums;

namespace RssReader.API.Modules;

public class TagsModule : BaseCarterModule
{
    public TagsModule() : base("/tags")
    {
        RequireAuthorization();
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("", GetAllTagsForUserAsync)
           .RequireAuthorization(new HasPermissionAttribute(Permissions.CrudTags));

        app.MapPost("", CreateTagAsync)
           .Produces(StatusCodes.Status401Unauthorized)
           .RequireAuthorization(new HasPermissionAttribute(Permissions.CrudTags));

        app.MapPut("{id}", UpdateTagAsync)
           .Produces(StatusCodes.Status401Unauthorized)
           .Produces(StatusCodes.Status404NotFound)
           .RequireAuthorization(new HasPermissionAttribute(Permissions.CrudTags));

        app.MapDelete("{id}", DeleteTagAsync)
           .Produces(StatusCodes.Status401Unauthorized)
           .Produces(StatusCodes.Status404NotFound)
           .RequireAuthorization(new HasPermissionAttribute(Permissions.CrudTags));

        app.MapGet("{id}/feedItems", GetFeedItemsForTagAsync)
           .Produces(StatusCodes.Status401Unauthorized)
           .Produces(StatusCodes.Status404NotFound)
           .RequireAuthorization(new HasPermissionAttribute(Permissions.ReadFeedItems));
    }

    private async Task<Ok<IList<Tag>>> GetAllTagsForUserAsync(
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var command = new GetAllTagsForUserQuery(GetRequesterId(httpContext));
        var tags = await sender.Send(command, cancellationToken);

        return TypedResults.Ok(tags);
    }

    private async Task<Created<Tag>> CreateTagAsync(
        UpsertTagRequest request, 
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var command = new CreateTagCommand(GetRequesterId(httpContext), request.Name);
        var tag = await sender.Send(command, cancellationToken);

        return TypedResults.Created(string.Empty, tag);
    }

    private async Task<Ok<Tag>> UpdateTagAsync(
        int id, 
        UpsertTagRequest request, 
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var command = new EditTagCommand(id, request.Name, GetRequesterId(httpContext));
        var updatedTag = await sender.Send(command, cancellationToken);

        return TypedResults.Ok(updatedTag);
    }

    private async Task<Ok<PaginatedResponse<DateTime, IList<FeedItem>>>> GetFeedItemsForTagAsync(
        int id,
        int pageSize,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken,
        DateTime? cursor)
    {
        var command = new GetAllFeedItemsForTagQuery(GetRequesterId(httpContext), id, pageSize, cursor);
        var feedItems = await sender.Send(command, cancellationToken);

        return TypedResults.Ok(feedItems);
    }

    private async Task<NoContent> DeleteTagAsync(
        int id,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var command = new DeleteTagCommand(GetRequesterId(httpContext), id);
        await sender.Send(command, cancellationToken);

        return TypedResults.NoContent();
    }
}
