using Microsoft.EntityFrameworkCore;
using RssReader.Application.Abstractions.Repositories;
using RssReader.Domain.Common;
using RssReader.Domain.Entities;

namespace RssReader.Infrastructure.Repositories;

internal class FeedItemsRepository : BaseSimpleRepository<int, FeedItem>, IFeedItemsRepository
{
    public FeedItemsRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> DoesInstanceExistForFeedAsync(
        int feedId, 
        string itemId, 
        string itemLink, 
        string itemTitle, 
        CancellationToken cancellationToken = default)
    {
        var query = _untrackedSet.Where(e => e.FeedId == feedId);

        query = query.Where(e => (!string.IsNullOrEmpty(itemId) && e.ItemId == itemId) ||
                                (!string.IsNullOrEmpty(itemLink) && e.Link == itemLink) ||
                                (!string.IsNullOrEmpty(itemTitle) && e.Title == itemTitle));

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<PaginatedEnumerable<DateTime, FeedItem>> GetAllForFeedAsync(
        int feedId, 
        int pageSize, 
        DateTime? cursor, 
        CancellationToken cancellationToken = default)
    {
        var query = _untrackedSet.Where(e => e.FeedId == feedId);

        return await GetCursorPaginationAsync(
            query, 
            e => e.PublishedAt ?? e.CreatedAt, 
            cursor, 
            pageSize, 
            getNewerValues: false,
            cancellationToken);
    }

    public async Task<PaginatedEnumerable<DateTime, FeedItem>> GetAllForFeedsAsync(
        int[] feedIds, 
        int pageSize, 
        DateTime? cursor, 
        CancellationToken cancellationToken = default)
    {
        var query = _untrackedSet.Where(e => feedIds.Contains(e.FeedId));

        return await GetCursorPaginationAsync(
            query,
            e => e.PublishedAt ?? e.CreatedAt,
            cursor,
            pageSize,
            getNewerValues: false,
            cancellationToken);
    }
}
