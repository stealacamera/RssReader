using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using RssReader.API.Common;
using RssReader.API.Common.Authorization;
using RssReader.Application.Behaviour.Operations.FeedItems.Queries.GetAllForFeed;
using RssReader.Application.Behaviour.Operations.FeedTags.Commands.AddTagToFeed;
using RssReader.Application.Behaviour.Operations.FeedTags.Commands.DeleteFeedTag;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Enums;

namespace RssReader.API.Modules;

public class FeedSubscriptionsModule : BaseCarterModule
{
    public FeedSubscriptionsModule() : base("feedSubscriptions")
    {
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("{subscriptionId}/tags", AddTagToFeedAsync)
           .WithSummary("Adds tag to a feed")
           .Produces(StatusCodes.Status401Unauthorized)
           .Produces(StatusCodes.Status404NotFound)
           .RequireAuthorization(new HasPermissionAttribute(Permissions.CrudFeedTags));

        app.MapGet("{id}/items", GetItemsForFeedAsync)
           .Produces(StatusCodes.Status401Unauthorized)
           .Produces(StatusCodes.Status404NotFound)
           .RequireAuthorization(new HasPermissionAttribute(Permissions.ReadFeedItems));

        app.MapDelete("{subscriptionId}/tags/{tagId}", RemoveTagFromFeedAsync)
           .WithSummary("Removes tag from a feed")
           .Produces(StatusCodes.Status401Unauthorized)
           .Produces(StatusCodes.Status404NotFound)
           .RequireAuthorization(new HasPermissionAttribute(Permissions.CrudFeedTags));
    }

    private async Task<Ok<PaginatedResponse<DateTime, IList<FeedItem>>>> GetItemsForFeedAsync(
        int id,
        int pageSize,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken,
        DateTime? cursor)
    {
        var request = new GetAllFeedItemsForFeedQuery(GetRequesterId(httpContext), id, pageSize, cursor);
        var items = await sender.Send(request, cancellationToken);

        return TypedResults.Ok(items);
    }

    private async Task<Ok> AddTagToFeedAsync(
        int subscriptionId,
        int tagId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var command = new AddTagToSubscriptionCommand(GetRequesterId(httpContext), tagId, subscriptionId);
        await sender.Send(command, cancellationToken);

        return TypedResults.Ok();
    }

    private async Task<NoContent> RemoveTagFromFeedAsync(
        int subscriptionId,
        int tagId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var command = new DeleteFeedTagCommand(GetRequesterId(httpContext), tagId, subscriptionId);
        await sender.Send(command, cancellationToken);

        return TypedResults.NoContent();
    }
}
