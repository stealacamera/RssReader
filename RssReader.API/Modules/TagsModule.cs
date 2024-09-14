using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using RssReader.API.Common.DTOs.Requests;
using RssReader.Application.Behaviour.Operations.Tags.Commands.Create;
using RssReader.Application.Behaviour.Operations.Tags.Commands.Edit;
using RssReader.Application.Common.DTOs;

namespace RssReader.API.Modules;

public class TagsModule : CarterModule
{
    public TagsModule() : base("/tags")
    {
        RequireAuthorization();
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("", CreateTag);
        app.MapPut("{id}", UpdateTag);
    }

    private async Task<Created<Tag>> CreateTag(UpsertTagRequest request, ISender sender)
    {
        var tag = await sender.Send(new CreateTagCommand(1, request.Name));
        return TypedResults.Created(string.Empty, tag);
    }

    private async Task<Ok<Tag>> UpdateTag(int id, UpsertTagRequest request, ISender sender)
    {
        var command = new EditTagCommand(id, request.Name, 1);
        var updatedTag = await sender.Send(command);

        return TypedResults.Ok(updatedTag);
    }
}
