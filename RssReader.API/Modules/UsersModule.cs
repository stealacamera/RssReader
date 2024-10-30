using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using RssReader.API.Common;
using RssReader.API.Common.DTOs.Requests;
using RssReader.Application.Behaviour.Operations.Users.Commands.Edit;

namespace RssReader.API.Modules;

public class UsersModule : BaseCarterModule
{
    public UsersModule() : base("/users")
    {
        RequireAuthorization();
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("", EditUserAsync)
           .WithDescription("Updates account information")
           .Produces(StatusCodes.Status401Unauthorized);
    }

    private async Task<Ok> EditUserAsync(
        EditUserRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var command = new EditUserCommand(GetRequesterId(httpContext), request.NewUsername);
        await sender.Send(command, cancellationToken);

        return TypedResults.Ok();
    }
}
