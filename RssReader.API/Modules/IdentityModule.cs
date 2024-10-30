using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RssReader.API.Common;
using RssReader.API.Common.DTOs.Requests;
using RssReader.Application.Behaviour.Operations.Identity.Commands.LogIn;
using RssReader.Application.Behaviour.Operations.Identity.Commands.Logout;
using RssReader.Application.Behaviour.Operations.Identity.Commands.ResendEmailVerification;
using RssReader.Application.Behaviour.Operations.Identity.Commands.UpdateTokens;
using RssReader.Application.Behaviour.Operations.Identity.Commands.VerifyOTP;
using RssReader.Application.Behaviour.Operations.Users.Commands.Create;
using RssReader.Application.Behaviour.Operations.Users.Commands.UpdatePassword;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Enums;
using RssReader.Application.Common.Exceptions;
using RssReader.Application.Common.Exceptions.General;

namespace RssReader.API.Modules;

public class IdentityModule : BaseCarterModule
{
    public IdentityModule() : base("/identity")
    {
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("signup", SignupAsync)
           .ProducesValidationProblem();

        app.MapPost("login", LoginAsync)
           .ProducesValidationProblem();

        app.MapPost("logout", LogoutAsync)
           .Produces(StatusCodes.Status404NotFound);

        app.MapPost("verification", VerifyEmailAsync)
           .WithSummary("Verifies account email")
           .Produces(StatusCodes.Status404NotFound)
           .Produces(StatusCodes.Status401Unauthorized)
           .ProducesValidationProblem();

        app.MapPut("verification", ResendEmailVerificationAsync)
           .WithSummary("Resends account verification token email")
           .Produces(StatusCodes.Status404NotFound);

        app.MapPost("tokens", RefreshTokensAsync)
           .WithSummary("Refreshes user tokens")
           .ProducesValidationProblem();

        app.MapPut("password", ChangePasswordAsync)
           .WithDescription("Updates account password");
    }

    /// <response code="400">Existing email or validation errors</response>
    private async Task<Results<Created<User>, BadRequest<ProblemDetails>>> SignupAsync(
        SignupRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new CreateUserCommand(request.Email, request.Password, Roles.User, request.Username);
            var user = await sender.Send(command, cancellationToken);

            return TypedResults.Created(string.Empty, user);
        }
        catch (ExistingEmailException)
        {
            return TypedResults.BadRequest(
                new ProblemDetails
                {
                    Title = "Existing email",
                    Detail = "There's an existing account using this email"
                });
        }
    }

    /// <response code="401">Email is unconfirmed</response>
    /// <response code="400">Incorrect credentials or validation errors</response>
    private async Task<Results<Ok<LoggedInUser>, BadRequest<ProblemDetails>>> LoginAsync(
        LoginRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new LoginCommand(request.Email, request.Password);
            var loggedInCredentials = await sender.Send(command, cancellationToken);

            return TypedResults.Ok(loggedInCredentials);
        }
        catch (InvalidLoginCredentialsException ex)
        {
            return TypedResults.BadRequest(
                new ProblemDetails { Title = "Invalid credentials", Detail = ex.Message });
        }
    }

    private async Task<Ok> LogoutAsync(
        ISender sender, 
        HttpContext httpContext, 
        CancellationToken cancellationToken)
    {
        var command = new LogoutCommand(GetRequesterId(httpContext));
        await sender.Send(command);

        return TypedResults.Ok();
    }

    /// <response code="404">User could not be found</response>
    /// <response code="400">Incorrect or invalid OTP, or validation errors</response>
    /// <response code="401">Email is already confirmed</response>
    private async Task<Results<Accepted, BadRequest<ProblemDetails>>> VerifyEmailAsync(
        EmailVerificationRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new VerifyOTPCommand(request.UserId, request.OTP);
            var isVerified = await sender.Send(command, cancellationToken);

            return isVerified ?
                   TypedResults.Accepted(string.Empty) :
                   TypedResults.BadRequest(new ProblemDetails { Title = "Incorrect OTP" });
        }
        catch (InvalidOTPException ex)
        {
            return TypedResults.BadRequest(
                new ProblemDetails { Title = "Invalid OTP", Detail = ex.Message });
        }
    }

    private async Task<Ok> ResendEmailVerificationAsync(
        int userId,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new ResendEmailVerificationCommand(userId);
        await sender.Send(command, cancellationToken);

        return TypedResults.Ok();
    }

    /// <response code="400">Invalid tokens, or validation errors</response>
    private async Task<Results<Ok<Tokens>, BadRequest<ProblemDetails>>> RefreshTokensAsync(
        RefreshTokensRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new UpdateTokensCommand(request.JwtToken, request.RefreshToken);
            var tokens = await sender.Send(command, cancellationToken);

            return TypedResults.Ok(tokens);
        }
        catch (UnauthorizedException)
        {
            return TypedResults.BadRequest(
                new ProblemDetails { 
                    Title = "Invalid credentials", 
                    Detail = "Invalid user token(s)" });
        }
        catch (ExpiredRefreshTokenException)
        {
            return TypedResults.BadRequest(
                new ProblemDetails { Title = "Expired refresh token" });
        }
    }

    private async Task<Results<Ok, BadRequest<ProblemDetails>>> ChangePasswordAsync(
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
        }
        catch (FailedPasswordVerification)
        {
            return TypedResults.BadRequest(
                new ProblemDetails
                {
                    Title = "Incorrect password",
                    Detail = "The password provided is incorrect"
                });
        }
    }
}
