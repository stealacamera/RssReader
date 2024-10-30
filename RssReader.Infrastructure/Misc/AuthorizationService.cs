using RssReader.Application.Abstractions;
using RssReader.Application.Common.Enums;

namespace RssReader.Infrastructure.Misc;

internal class AuthorizationService : IAuthorizationService
{
    private readonly IWorkUnit _workUnit;

    public AuthorizationService(IWorkUnit workUnit)
        => _workUnit = workUnit;

    public async Task<bool> IsUserAuthorizedAsync(
        int userId,
        Permissions permission,
        CancellationToken cancellationToken = default)
    {
        var userRole = await _workUnit.UserRolesRepository
                                     .GetRoleForUserAsync(userId, cancellationToken);

        if (userRole == null)
            return false;

        return await _workUnit.RolePermissionsRepository
                              .DoesInstanceExistAsync(
                                   (int)userRole,
                                   (int)permission,
                                   cancellationToken);
    }
}
