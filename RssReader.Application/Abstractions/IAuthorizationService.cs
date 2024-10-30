using RssReader.Application.Common.Enums;

namespace RssReader.Application.Abstractions;

public interface IAuthorizationService
{
    Task<bool> IsUserAuthorizedAsync(int userId, Permissions permission, CancellationToken cancellationToken = default);
}
