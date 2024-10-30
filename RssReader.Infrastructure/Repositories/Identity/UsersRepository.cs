using Microsoft.EntityFrameworkCore;
using RssReader.Application.Abstractions.Repositories.Identity;
using RssReader.Domain.Entities.Identity;

namespace RssReader.Infrastructure.Repositories.Identity;

internal class UsersRepository : BaseSimpleRepository<int, User>, IUsersRepository
{
    public UsersRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var query = _set.Where(e => e.Email == email);
        return await query.FirstOrDefaultAsync(cancellationToken);
    }
}
