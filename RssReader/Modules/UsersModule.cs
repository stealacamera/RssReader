using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using RssReader.API.Common.DTOs;
using RssReader.Application.Behaviour.Users.Commands.Create;
using RssReader.Application.Behaviour.Users.Commands.Edit;
using RssReader.Application.Behaviour.Users.Commands.UpdatePassword;
using RssReader.Application.Behaviour.Users.Queries.LogIn;

namespace RssReader.API.Modules;

public class UsersModule : CarterModule
{
    public UsersModule() : base("/users")
    {        
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/login", async (LoginRequest request, ISender sender) =>
        {
            await sender.Send(new LoginQuery(request.Email, request.Password));
            // TODO add auth

            return TypedResults.Ok();
        });

        app.MapPost("/signup", async (SignupRequest request, ISender sender) =>
        {
            await sender.Send(new CreateUserCommand(request.Email, request.Password, request.Username));
            // TODO add auth

            return TypedResults.Created();
        });

        app.MapPatch("/edit", async (EditUserRequest request, ISender sender) =>
        {
            await sender.Send(new EditUserCommand(1, request.NewUsername));
            return TypedResults.Ok();
        });

        app.MapPatch("/changePassword", async (ChangePasswordRequest request, ISender sender) =>
        {
            await sender.Send(new UpdatePasswordCommand(1, request.OldPassword, request.NewPassword));
            return TypedResults.Ok();
        });
    }
}
