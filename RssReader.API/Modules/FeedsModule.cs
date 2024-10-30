using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using RssReader.API.Common;
using RssReader.API.Common.Authorization;
using RssReader.API.Common.DTOs.Requests;
using RssReader.Application.Behaviour.Operations.Feeds.Commands.Create;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Enums;

namespace RssReader.API.Modules;

public class FeedsModule : BaseCarterModule
{
    public FeedsModule() : base("/feeds")
    {
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("", CreateAsync)
           .Produces(StatusCodes.Status401Unauthorized)
           .Produces(StatusCodes.Status404NotFound)
           .RequireAuthorization(new HasPermissionAttribute(Permissions.CrudFeeds));

        // TODO add get all

        // TODO delete one
    }

    private async Task<Created<Feed>> CreateAsync(
        CreateFeedRequest request, 
        ISender sender, 
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var command = new CreateFeedCommand(GetRequesterId(httpContext), request.Url, request.Name);
        var feed = await sender.Send(command, cancellationToken);

        return TypedResults.Created(string.Empty, feed);
    }    
}
