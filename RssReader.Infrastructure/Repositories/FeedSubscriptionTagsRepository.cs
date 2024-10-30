using Microsoft.EntityFrameworkCore;
using RssReader.Application.Abstractions.Repositories;
using RssReader.Domain.Entities;

namespace RssReader.Infrastructure.Repositories;

internal class FeedSubscriptionTagsRepository : BaseRepository<FeedSubscriptionTag>, IFeedSubscriptionTagsRepository
{
    public FeedSubscriptionTagsRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task DeleteAllForFeedSubscriptionAsync(int feedSubscriptionId, CancellationToken cancellationToken = default)
        => await _set.Where(e => e.FeedSubscriptionId == feedSubscriptionId)
                     .ExecuteDeleteAsync(cancellationToken);

    public async Task<bool> DoesInstanceExistAsync(int tagId, int feedSubscriptionId, CancellationToken cancellationToken = default)
        => await GetByIdsAsync(tagId, feedSubscriptionId, cancellationToken) != null;

    public async Task<IEnumerable<FeedSubscriptionTag>> GetAllForFeedSubscriptionAsync(int feedSubscriptionId, CancellationToken cancellationToken = default)
    {
        var query = _untrackedSet.Where(e => e.FeedSubscriptionId == feedSubscriptionId);
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<FeedSubscriptionTag>> GetAllForTagAsync(int tagId, CancellationToken cancellationToken = default)
    {
        var query = _untrackedSet.Where(e => e.TagId == tagId);
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<FeedSubscriptionTag?> GetByIdsAsync(int tagId, int feedSubscriptionId, CancellationToken cancellationToken = default)
        => await _set.FindAsync([tagId, feedSubscriptionId], cancellationToken: cancellationToken);
}
