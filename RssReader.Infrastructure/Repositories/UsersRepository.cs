using Microsoft.EntityFrameworkCore;
using RssReader.Application.Abstractions.Repositories;
using RssReader.Domain.Entities;

namespace RssReader.Infrastructure.Repositories;

internal class UsersRepository : BaseSimpleRepository<User>, IUsersRepository
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
