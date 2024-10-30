using RssReader.Application.Common.Enums;
using RssReader.Domain.Entities.Identity;

namespace RssReader.Application.Abstractions.Repositories.Identity;

public interface IUserRolesRepository : IBaseRepository<UserRole>
{
    Task<Roles?> GetRoleForUserAsync(int userId, CancellationToken cancellationToken = default);
}
