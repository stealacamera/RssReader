using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using RssReader.Application.Abstractions;
using RssReader.Application.Common.Enums;
using System.IdentityModel.Tokens.Jwt;

namespace RssReader.API.Common.Authorization;

internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
        => _serviceScopeFactory = serviceScopeFactory;

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        string? userId = context.User
                                .Claims
                                .Where(x => x.Type == JwtRegisteredClaimNames.Sub)
                                .FirstOrDefault()?
                                .Value;

        if (!int.TryParse(userId, out int parsedUserId))
            return;

        using IServiceScope scope = _serviceScopeFactory.CreateScope();
        
        var authService = scope.ServiceProvider
                               .GetRequiredService<Application.Abstractions.IAuthorizationService>();

        if (await authService.IsUserAuthorizedAsync(
            parsedUserId, 
            Enum.Parse<Permissions>(requirement.Permission)))
        {
            context.Succeed(requirement);
            return;
        }
        
        context.Fail();
    }
}
