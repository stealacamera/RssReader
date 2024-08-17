using Carter;
using MediatR;
using RssReader.API.Common.DTOs.Requests;
using RssReader.Application.Behaviour.Operations.Folders.Commands.Create;
using RssReader.Application.Behaviour.Operations.Folders.Queries.GetAllForUser;
using RssReader.Application.Behaviour.Operations.Folders.Queries.GetChildrenFolders;

namespace RssReader.API.Modules;

public class FoldersModule : CarterModule
{
    public FoldersModule() : base("/folders")
    {
        RequireAuthorization();
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        int userId = 1;
        
        app.MapGet("/user/{id}", async (int id, ISender sender) =>
        {
            var folders = await sender.Send(new GetAllFoldersForUserQuery(userId));
            return TypedResults.Ok(folders);
        });

        app.MapGet("/{id}/children", async (int id, ISender sender) =>
        {
            var subFolders = await sender.Send(new GetChildrenFoldersQuery(userId, id));
            return TypedResults.Ok(subFolders);
        });

        app.MapPost("/", async (CreateFolderRequest request, ISender sender) =>
        {
            var command = new CreateFolderCommand(userId, request.Name, request.ParentFolderId);
            var newFolder = await sender.Send(command);

            return TypedResults.Created(string.Empty, newFolder);
        });
    }
}
