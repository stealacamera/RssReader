using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using RssReader.API.Common;
using RssReader.API.Common.Authorization;
using RssReader.Application.Behaviour.Operations.FeedItems.Queries.GetAllForUser;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Enums;

namespace RssReader.API.Modules;

public class FeedItemsModule : BaseCarterModule
{
    public FeedItemsModule() : base("feedItems")
    {
        RequireAuthorization();
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("", GetAllForUserAsync)
           .WithSummary("Gets all items from all of the user's feeds")
           .RequireAuthorization(new HasPermissionAttribute(Permissions.ReadFeedItems));
    }

    private async Task<Ok<PaginatedResponse<DateTime, IList<FeedItem>>>> GetAllForUserAsync(
        int pageSize,
        ISender sender, 
        HttpContext httpContext, 
        CancellationToken cancellationToken,
        DateTime? cursor)
    {
        var request = new GetAllFeedItemsForUserQuery(GetRequesterId(httpContext), pageSize, cursor);
        var feedItems = await sender.Send(request, cancellationToken);

        return TypedResults.Ok(feedItems);
    }
}
