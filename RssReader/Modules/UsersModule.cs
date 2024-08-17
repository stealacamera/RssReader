using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using RssReader.API.Common.DTOs;
using RssReader.API.Common.DTOs.Requests;
using RssReader.Application.Behaviour.Operations.Identity.Commands.UpdateTokens;
using RssReader.Application.Behaviour.Operations.Identity.Commands.VerifyOTP;
using RssReader.Application.Behaviour.Operations.Users.Commands.Create;
using RssReader.Application.Behaviour.Operations.Users.Commands.Edit;
using RssReader.Application.Behaviour.Operations.Users.Commands.LogIn;
using RssReader.Application.Behaviour.Operations.Users.Commands.UpdatePassword;
using RssReader.Application.Common.DTOs.Notifications;

namespace RssReader.API.Modules;

public class UsersModule : CarterModule
{
    public UsersModule() : base("/users")
    {
        RequireAuthorization();
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        // TODO create guest user as default for non-signups?
        app.MapPost("/login", async (LoginRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new LoginCommand(request.Email, request.Password);
            var loggedInCredentials = await sender.Send(command, cancellationToken);

            return TypedResults.Ok(loggedInCredentials);
        })
           .AllowAnonymous();

        app.MapPost("/signup", async (SignupRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new CreateUserCommand(request.Email, request.Password, request.Username);
            var user = await sender.Send(command, cancellationToken);

            return TypedResults.Created(string.Empty, user);
        })
           .AllowAnonymous();

        app.MapPost(
            "{id}/refreshToken",
            async (int id, RefreshTokenModel request, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new UpdateTokensCommand(id, request.JwtToken, request.RefreshToken);
                var tokens = await sender.Send(command, cancellationToken);

                return TypedResults.Ok(tokens);
            });

        app.MapPost(
            "{id}/verifyEmail", 
            async Task<Results<Accepted, BadRequest>> (int id, string otp, ISender sender, CancellationToken cancellationToken) =>
            {
                var isVerified = await sender.Send(new VerifyOTPCommand(id, otp), cancellationToken);

                return isVerified ?
                       TypedResults.Accepted(string.Empty) :
                       TypedResults.BadRequest();
            });

        app.MapPost("{id}/resendEmailVerification", async (int id, IPublisher publisher) =>
        {
            await publisher.Publish(new UserCreatedNotification(id));
            return TypedResults.Ok();
        });

        app.MapPatch(
            "{id}/edit", 
            async (int id, EditUserRequest request, ISender sender, CancellationToken cancellationToken) =>
            {
                await sender.Send(new EditUserCommand(id, request.NewUsername), cancellationToken);
                return TypedResults.Ok();
            });

        app.MapPut(
            "{id}/changePassword", 
            async (int id, ChangePasswordRequest request, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new UpdatePasswordCommand(id, request.OldPassword, request.NewPassword);
                await sender.Send(command, cancellationToken);

                return TypedResults.Ok();
            });
    }
}
