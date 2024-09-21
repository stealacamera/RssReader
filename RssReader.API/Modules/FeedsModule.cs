using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using RssReader.API.Common;
using RssReader.API.Common.DTOs.Requests;
using RssReader.Application.Behaviour.Operations.Feeds.Commands.Create;
using RssReader.Application.Behaviour.Operations.FeedTags.Commands.AddTagToFeed;
using RssReader.Application.Behaviour.Operations.FeedTags.Commands.DeleteFeedTag;
using RssReader.Application.Common.DTOs;

namespace RssReader.API.Modules;

public class FeedsModule : BaseCarterModule
{
    public FeedsModule() : base("/feeds")
    {
        RequireAuthorization();
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("", Create)
           .Produces(StatusCodes.Status401Unauthorized)
           .Produces(StatusCodes.Status404NotFound);

        app.MapPost("{id}/tags", AddTagToFeed)
           .WithSummary("Adds tag to a feed")
           .Produces(StatusCodes.Status401Unauthorized)
           .Produces(StatusCodes.Status404NotFound);

        app.MapDelete("{feedId}/tags/{tagId}", RemoveTagFromFeed)
           .WithSummary("Removes tag from a feed")
           .Produces(StatusCodes.Status401Unauthorized)
           .Produces(StatusCodes.Status404NotFound);
    }

    private async Task<Created<Feed>> Create(
        CreateFeedRequest request, 
        ISender sender, 
        HttpContext httpContext)
    {
        var command = new CreateFeedCommand(GetRequesterId(httpContext), request.FolderId, request.Url, request.Name);
        var feed = await sender.Send(command);

        return TypedResults.Created(string.Empty, feed);
    }

    private async Task<Ok> AddTagToFeed(
        int id, 
        int tagId, 
        ISender sender, 
        HttpContext httpContext)
    {
        var command = new AddTagToFeedCommand(GetRequesterId(httpContext), tagId, id);
        await sender.Send(command);

        return TypedResults.Ok();
    }

    private async Task<NoContent> RemoveTagFromFeed(
        int feedId, 
        int tagId, 
        ISender sender,
        HttpContext httpContext)
    {
        var command = new DeleteFeedTagCommand(GetRequesterId(httpContext), tagId, feedId);
        await sender.Send(command);

        return TypedResults.NoContent();
    }
}
