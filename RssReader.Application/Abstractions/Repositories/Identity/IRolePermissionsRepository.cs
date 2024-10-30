using RssReader.Domain.Entities.Identity;

namespace RssReader.Application.Abstractions.Repositories.Identity;

public interface IRolePermissionsRepository : IBaseRepository<RolePermission>
{
    Task<bool> DoesInstanceExistAsync(
        int roleId, 
        int permissionId, 
        CancellationToken cancellationToken = default);
}
