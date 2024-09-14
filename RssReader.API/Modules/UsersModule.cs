using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using RssReader.API.Common.DTOs.Requests;
using RssReader.Application.Behaviour.Operations.Folders.Queries.GetAllForUser;
using RssReader.Application.Behaviour.Operations.Tags.Queries.GetAllForUser;
using RssReader.Application.Behaviour.Operations.Users.Commands.Edit;
using RssReader.Application.Behaviour.Operations.Users.Commands.UpdatePassword;
using RssReader.Application.Common.DTOs;

namespace RssReader.API.Modules;

public class UsersModule : CarterModule
{
    public UsersModule() : base("/users")
    {
        RequireAuthorization();
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("{id}", EditUser)
           .WithDescription("Updates account information");

        app.MapPut("{id}/password", ChangePassword)
           .WithDescription("Updates account password");

        app.MapGet("{id}/tags", GetAllTagsForUser);
        app.MapGet("{id}/folders", GetAllFoldersForUser);
    }
    
    private async Task<Ok> EditUser(int id, EditUserRequest request, ISender sender, CancellationToken cancellationToken)
    {
        await sender.Send(new EditUserCommand(id, request.NewUsername), cancellationToken);
        return TypedResults.Ok();
    }

    private async Task<Ok> ChangePassword(int id, ChangePasswordRequest request, ISender sender, CancellationToken cancellationToken)
    {
        var command = new UpdatePasswordCommand(id, request.OldPassword, request.NewPassword);
        await sender.Send(command, cancellationToken);

        return TypedResults.Ok();
    }

    private async Task<Ok<IList<Tag>>> GetAllTagsForUser(int id, ISender sender)
    {
        var tags = await sender.Send(new GetAllTagsForUserQuery(id));
        return TypedResults.Ok(tags);
    }

    private async Task<Ok<IList<Folder>>> GetAllFoldersForUser(int id, ISender sender)
    {
        var folders = await sender.Send(new GetAllFoldersForUserQuery(1, id));
        return TypedResults.Ok(folders);
    }
}
