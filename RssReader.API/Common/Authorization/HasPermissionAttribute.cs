using Microsoft.AspNetCore.Authorization;
using RssReader.Application.Common.Enums;

namespace RssReader.API.Common.Authorization;

public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(Permissions permission)
        : base(policy: permission.ToString())
    {
    }
}