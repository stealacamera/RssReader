using Microsoft.EntityFrameworkCore;
using RssReader.Application.Abstractions.Repositories;
using RssReader.Domain.Entities;

namespace RssReader.Infrastructure.Repositories;

internal class FeedsRepository : BaseSimpleRepository<int, Feed>, IFeedsRepository
{
    public FeedsRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<int[]> GetAllIdsAsync(CancellationToken cancellationToken = default)
        => await _untrackedSet.Select(e => e.Id).ToArrayAsync(cancellationToken);

    public async Task<Feed?> GetByUrlAsync(string url, CancellationToken cancellationToken = default)
    {
        var query = _untrackedSet.Where(e => e.Url == url);
        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsUrlRegisteredAsync(string url, CancellationToken cancellationToken = default)
    {
        var query = _untrackedSet.Where(e => e.Url == url);
        return await query.AnyAsync(cancellationToken);
    }
}
