
using RssReader.Domain.Common;
using RssReader.Domain.Entities;

namespace RssReader.Application.Abstractions.Repositories;

public interface IFeedItemsRepository : IBaseSimpleRepository<int, FeedItem>
{
    Task<PaginatedEnumerable<DateTime, FeedItem>> GetAllForFeedAsync(
        int feedId, 
        int pageSize, 
        DateTime? cursor, 
        CancellationToken cancellationToken = default);
    
    Task<PaginatedEnumerable<DateTime, FeedItem>> GetAllForFeedsAsync(
        int[] feedIds, 
        int pageSize, 
        DateTime? cursor, 
        CancellationToken cancellationToken = default);

    Task<bool> DoesInstanceExistForFeedAsync(
        int feedId, 
        string itemId, 
        string itemLink, 
        string itemTitle, 
        CancellationToken cancellationToken = default);
}
