using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using RssReader.API.Common.DTOs.Requests;
using RssReader.Application.Behaviour.Operations.Feeds.Commands.Create;
using RssReader.Application.Behaviour.Operations.FeedTags.Commands.AddTagToFeed;
using RssReader.Application.Behaviour.Operations.FeedTags.Commands.DeleteFeedTag;
using RssReader.Application.Common.DTOs;

namespace RssReader.API.Modules;

public class FeedsModule : CarterModule
{
    public FeedsModule() : base("/feeds")
    {
        RequireAuthorization();
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("", Create);

        app.MapPost("{id}/tags", AddTagToFeed);

        // todo ? separate endpoint or given along with feed entity
        app.MapDelete("{feedId}/tags/{tagId}", RemoveTagFromFeed);
    }

    private async Task<Created<Feed>> Create(CreateFeedRequest request, ISender sender)
    {
        var command = new CreateFeedCommand(1, request.FolderId, request.Url, request.Name);
        var feed = await sender.Send(command);

        return TypedResults.Created(string.Empty, feed);
    }

    private async Task<Ok> AddTagToFeed(int id, int tagId, ISender sender)
    {
        var command = new AddTagToFeedCommand(1, tagId, id);
        await sender.Send(command);

        return TypedResults.Ok();
    }

    private async Task<NoContent> RemoveTagFromFeed(int feedId, int tagId, ISender sender)
    {
        var command = new DeleteFeedTagCommand(1, tagId, feedId);
        await sender.Send(command);

        return TypedResults.NoContent();
    }
}
