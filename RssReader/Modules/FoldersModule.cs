using Carter;
using MediatR;
using RssReader.Application.Behaviour.Folders.Commands.Create;
using RssReader.Application.Behaviour.Folders.Queries.GetAllForUser;
using RssReader.Application.Behaviour.Folders.Queries.GetChildrenFolders;
using RssReader.Application.Common.DTOs;

namespace RssReader.API.Modules;

public class FoldersModule : CarterModule
{
    public FoldersModule() : base("/folders")
    {
        //this.RequireAuthorization();
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        int userId = 1;
        
        app.MapGet("/user/{id}", async (int id, ISender sender) =>
        {
            var folders = await sender.Send(new GetAllFoldersForUserQuery(userId));
            return folders;
        });

        app.MapGet("/{id}/children", async (int id, ISender sender) =>
        {
            var subFolders = await sender.Send(new GetChildrenFoldersQuery(userId, id));
            return subFolders;
        });

        app.MapPost("/", async (CreateFolderRequest request, ISender sender) =>
        {
            var command = new CreateFolderCommand(userId, request.Name, request.ParentFolderId);
            var newFolder = await sender.Send(command);

            return newFolder;
        });
    }
}
