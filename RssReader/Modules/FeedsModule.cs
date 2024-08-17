using Carter;
using MediatR;
using RssReader.API.Common.DTOs.Requests;
using RssReader.Application.Behaviour.Operations.Feeds.Commands.Create;
using RssReader.Application.Behaviour.Operations.Feeds.Queries.GetAllForFolder;
using RssReader.Application.Behaviour.Operations.FeedTags.Commands.AddTagToFeed;
using RssReader.Application.Behaviour.Operations.FeedTags.Commands.DeleteFeedTag;

namespace RssReader.API.Modules;

public class FeedsModule : CarterModule
{
    public FeedsModule() : base("/feeds")
    {
        RequireAuthorization();
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        int userId = 1;

        app.MapPost("/", async (CreateFeedRequest request, ISender sender) =>
        {
            var command = new CreateFeedCommand(userId, request.FolderId, request.Url, request.Name);
            return TypedResults.Created(string.Empty, await sender.Send(command));
        });

        app.MapGet("/folder/{id}", async (int id, ISender sender) =>
        {
            var command = new GetAllFeedsForFolderQuery(id, userId);
            return TypedResults.Ok(await sender.Send(command));
        });

        app.MapPost("/{id}/tags", async (int id, int tagId, ISender sender) =>
        {
            var command = new AddTagToFeedCommand(userId, tagId, id);
            await sender.Send(command);

            return TypedResults.Ok();
        });

        app.MapDelete("/{feedId}/tags/{tagId}", async (int feedId, int tagId, ISender sender) =>
        {
            var command = new DeleteFeedTagCommand(userId, tagId, feedId);
            await sender.Send(command);

            return TypedResults.NoContent();
        });
    }
}
