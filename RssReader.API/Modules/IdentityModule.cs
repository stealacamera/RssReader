using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RssReader.API.Common.DTOs.Requests;
using RssReader.Application.Behaviour.Operations.Identity.Commands.UpdateTokens;
using RssReader.Application.Behaviour.Operations.Identity.Commands.VerifyOTP;
using RssReader.Application.Behaviour.Operations.Users.Commands.Create;
using RssReader.Application.Behaviour.Operations.Users.Commands.LogIn;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.DTOs.Notifications;

namespace RssReader.API.Modules;

public class IdentityModule : CarterModule
{
    public IdentityModule() : base("/identity")
    {
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        // TODO create guest user as default for non-signups?

        app.MapPost("signup", Signup);
        app.MapPost("login", Login);

        app.MapPost("verification", VerifyEmail)
           .WithDescription("Verifies account email");
        
        app.MapPut("verification", ResendEmailVerification)
           .WithDescription("Resends account verification token email");

        app.MapPost("tokens", RefreshTokens)
           .WithDescription("Refreshes user tokens");
    }

    private async Task<Ok<LoggedInUser>> Login(LoginRequest request, ISender sender, CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Email, request.Password);
        var loggedInCredentials = await sender.Send(command, cancellationToken);

        return TypedResults.Ok(loggedInCredentials);
    }

    private async Task<Created<User>> Signup(SignupRequest request, ISender sender, CancellationToken cancellationToken)
    {
        var command = new CreateUserCommand(request.Email, request.Password, request.Username);
        var user = await sender.Send(command, cancellationToken);

        return TypedResults.Created(string.Empty, user);
    }

    private async Task<Results<Accepted, BadRequest>> VerifyEmail(EmailVerificationRequest request, ISender sender, CancellationToken cancellationToken)
    {
        var isVerified = await sender.Send(new VerifyOTPCommand(request.UserId, request.OTP), cancellationToken);

        return isVerified ?
               TypedResults.Accepted(string.Empty) :
               TypedResults.BadRequest();
    }

    private async Task<Ok<Tokens>> RefreshTokens(RefreshTokensRequest request, ISender sender, CancellationToken cancellationToken)
    {
        var command = new UpdateTokensCommand(request.UserId, request.JwtToken, request.RefreshToken);
        var tokens = await sender.Send(command, cancellationToken);

        return TypedResults.Ok(tokens);
    }

    private async Task<Ok> ResendEmailVerification([FromBody] int userId, IPublisher publisher, CancellationToken cancellationToken)
    {
        await publisher.Publish(new UserCreatedNotification(userId), cancellationToken);
        return TypedResults.Ok();
    }
}
