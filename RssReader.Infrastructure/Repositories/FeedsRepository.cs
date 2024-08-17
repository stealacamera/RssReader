using Microsoft.EntityFrameworkCore;
using RssReader.Application.Abstractions.Repositories;
using RssReader.Domain.Entities;

namespace RssReader.Infrastructure.Repositories;

internal class FeedsRepository : BaseSimpleRepository<Feed>, IFeedsRepository
{
    public FeedsRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IList<Feed>> GetAllForFolderAsync(int folderId, CancellationToken cancellationToken = default)
    {
        var query = _set.Where(e => e.FolderId == folderId);
        return await query.ToListAsync(cancellationToken);
    }
}
