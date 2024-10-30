using Microsoft.AspNetCore.Authorization;

namespace RssReader.API.Common.Authorization;

internal class PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement(string permission)
        => Permission = permission;

    public string Permission { get; }
}
