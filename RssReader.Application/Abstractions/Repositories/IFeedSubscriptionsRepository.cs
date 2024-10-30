using RssReader.Domain.Entities;

namespace RssReader.Application.Abstractions.Repositories;

public interface IFeedSubscriptionsRepository : IBaseSimpleRepository<int, FeedSubscription>
{
    Task<IEnumerable<FeedSubscription>> GetAllForUserAsync(int userId, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<FeedSubscription>> GetAllForFolderAsync(int folderId, CancellationToken cancellationToken = default);
    Task DeleteAllForFolderAsync(int folderId, CancellationToken cancellationToken = default);

    Task<bool> DoesInstanceExistAsync(int feedId, int folderId, CancellationToken cancellationToken = default);
}
