using Microsoft.EntityFrameworkCore;
using RssReader.Application.Abstractions.Repositories;
using RssReader.Domain.Entities;

namespace RssReader.Infrastructure.Repositories;

internal class FeedSubscriptionsRepository : BaseSimpleRepository<int, FeedSubscription>, IFeedSubscriptionsRepository
{
    public FeedSubscriptionsRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task DeleteAllForFolderAsync(int folderId, CancellationToken cancellationToken = default)
        => await _set.Where(e => e.FolderId == folderId)
                  .ExecuteDeleteAsync(cancellationToken);

    public async Task<bool> DoesInstanceExistAsync(int feedId, int folderId, CancellationToken cancellationToken = default)
    {
        var query = _untrackedSet.Where(e => e.FeedId == feedId && e.FolderId == folderId);
        return await query.AnyAsync();
    }

    public async Task<IEnumerable<FeedSubscription>> GetAllForFolderAsync(int folderId, CancellationToken cancellationToken = default)
    {
        var query = _untrackedSet.Where(e => e.FolderId == folderId);
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<FeedSubscription>> GetAllForUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        var query = _untrackedSet.Where(e => e.Folder.OwnerId == userId);
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<FeedSubscription?> GetByIdAsync(int feedSubscriptionId, CancellationToken cancellationToken = default)
        => await _set.FindAsync(feedSubscriptionId, cancellationToken);
}
