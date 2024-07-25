﻿using Carter;
using MediatR;
using RssReader.Application.Behaviour.Tags.Commands.Create;
using RssReader.Application.Behaviour.Tags.Commands.Edit;
using RssReader.Application.Behaviour.Tags.Queries.GetAllForUser;
using RssReader.Application.Common.DTOs;

namespace RssReader.API.Modules;

public class TagsModule : CarterModule
{
    public TagsModule() : base("/tags")
    {
        
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        int userId = 1;

        app.MapPost("/", async (UpsertTagRequest request, ISender sender) =>
        {
            var command = new CreateTagCommand(userId, request.Name);
            return await sender.Send(request);
        });

        app.MapPatch("/{id}", async(int id, UpsertTagRequest request, ISender sender) =>
        {
            var command = new EditTagCommand(id, request.Name, userId);
            return await sender.Send(request);
        });

        app.MapGet("/user/{id}", async (int id, ISender sender) =>
        {
            return await sender.Send(new GetAllTagsForUserQuery(id));
        });
    }
}
