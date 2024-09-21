using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using RssReader.API.Common;
using RssReader.API.Common.DTOs.Requests;
using RssReader.Application.Behaviour.Operations.Tags.Commands.Create;
using RssReader.Application.Behaviour.Operations.Tags.Commands.Edit;
using RssReader.Application.Common.DTOs;

namespace RssReader.API.Modules;

public class TagsModule : BaseCarterModule
{
    public TagsModule() : base("/tags")
    {
        RequireAuthorization();
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("", CreateTag)
           .Produces(StatusCodes.Status401Unauthorized);

        app.MapPut("{id}", UpdateTag)
           .Produces(StatusCodes.Status401Unauthorized)
           .Produces(StatusCodes.Status404NotFound);
    }

    private async Task<Created<Tag>> CreateTag(
        UpsertTagRequest request, 
        ISender sender,
        HttpContext httpContext)
    {
        var command = new CreateTagCommand(GetRequesterId(httpContext), request.Name);
        var tag = await sender.Send(command);

        return TypedResults.Created(string.Empty, tag);
    }

    private async Task<Ok<Tag>> UpdateTag(
        int id, 
        UpsertTagRequest request, 
        ISender sender,
        HttpContext httpContext)
    {
        var command = new EditTagCommand(id, request.Name, GetRequesterId(httpContext));
        var updatedTag = await sender.Send(command);

        return TypedResults.Ok(updatedTag);
    }
}
