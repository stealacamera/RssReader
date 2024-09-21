using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RssReader.API.Common;
using RssReader.API.Common.DTOs.Requests;
using RssReader.Application.Behaviour.Operations.Folders.Queries.GetAllForUser;
using RssReader.Application.Behaviour.Operations.Tags.Queries.GetAllForUser;
using RssReader.Application.Behaviour.Operations.Users.Commands.Edit;
using RssReader.Application.Behaviour.Operations.Users.Commands.UpdatePassword;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions;

namespace RssReader.API.Modules;

public class UsersModule : BaseCarterModule
{
    public UsersModule() : base("/users")
    {
        RequireAuthorization();
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("", EditUser)
           .WithDescription("Updates account information")
           .Produces(StatusCodes.Status401Unauthorized);

        app.MapPut("password", ChangePassword)
           .WithDescription("Updates account password");

        app.MapGet("tags", GetAllTagsForUser);
        app.MapGet("folders", GetAllFoldersForUser);
    }

    private async Task<Ok> EditUser(
        EditUserRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var command = new EditUserCommand(GetRequesterId(httpContext), request.NewUsername);
        await sender.Send(command, cancellationToken);

        return TypedResults.Ok();
    }

    private async Task<Results<Ok, BadRequest<ProblemDetails>>> ChangePassword(
        ChangePasswordRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new UpdatePasswordCommand(GetRequesterId(httpContext), request.OldPassword, request.NewPassword);
            await sender.Send(command, cancellationToken);

            return TypedResults.Ok();
        } catch(FailedPasswordVerification)
        {
            return TypedResults.BadRequest(
                new ProblemDetails
                {
                    Title = "Incorrect password",
                    Detail = "The password provided is incorrect"
                });
        }
    }

    private async Task<Ok<IList<Tag>>> GetAllTagsForUser(ISender sender, HttpContext httpContext)
    {
        var command = new GetAllTagsForUserQuery(GetRequesterId(httpContext));
        var tags = await sender.Send(command);

        return TypedResults.Ok(tags);
    }

    private async Task<Ok<IList<Folder>>> GetAllFoldersForUser(ISender sender, HttpContext httpContext)
    {
        var query = new GetAllFoldersForUserQuery(GetRequesterId(httpContext));
        var folders = await sender.Send(query);

        return TypedResults.Ok(folders);
    }
}
