using RssReader.Application.Abstractions.Repositories.Identity;
using RssReader.Domain.Entities.Identity;

namespace RssReader.Infrastructure.Repositories.Identity;

internal class RolePermissionsRepository : BaseRepository<RolePermission>, IRolePermissionsRepository
{
    public RolePermissionsRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> DoesInstanceExistAsync(int roleId, int permissionId, CancellationToken cancellationToken = default)
        => await _set.FindAsync(roleId, permissionId, cancellationToken) != null;
}
