using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RssReader.Application.Abstractions.Repositories.Identity;
using RssReader.Application.Common.Enums;
using RssReader.Domain.Entities.Identity;

namespace RssReader.Infrastructure.Repositories.Identity;

internal class UserRolesRepository : BaseRepository<UserRole>, IUserRolesRepository
{
    private readonly IDistributedCache _distributedCache;

    public UserRolesRepository(AppDbContext dbContext, IDistributedCache distributedCache) : base(dbContext)
        => _distributedCache = distributedCache;

    public async Task<Roles?> GetRoleForUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        string cacheKey = $"user-{userId}-role";
        string? cachedUserRole = await _distributedCache.GetStringAsync(cacheKey, cancellationToken);
        UserRole? role;

        if (string.IsNullOrWhiteSpace(cachedUserRole))
        {
            var query = _untrackedSet.Where(e => e.UserId == userId);
            role = await query.FirstAsync(cancellationToken);

            if (role == null)
                return null;

            await _distributedCache.SetStringAsync(
                cacheKey,
                JsonConvert.SerializeObject(role),
                cancellationToken);
        }
        else
            role = JsonConvert.DeserializeObject<UserRole>(cachedUserRole)!;
        
        return (Roles)role.RoleId;
    }
}
