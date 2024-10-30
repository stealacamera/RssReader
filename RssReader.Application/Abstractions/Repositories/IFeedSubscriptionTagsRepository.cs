using RssReader.Domain.Entities;

namespace RssReader.Application.Abstractions.Repositories;

public interface IFeedSubscriptionTagsRepository : IBaseRepository<FeedSubscriptionTag>
{
    Task<FeedSubscriptionTag?> GetByIdsAsync(int tagId, int feedSubscriptionId, CancellationToken cancellationToken = default);
    Task<bool> DoesInstanceExistAsync(int tagId, int feedSubscriptionId, CancellationToken cancellationToken = default);

    Task<IEnumerable<FeedSubscriptionTag>> GetAllForFeedSubscriptionAsync(int feedSubscriptionId, CancellationToken cancellationToken = default);
    Task<IEnumerable<FeedSubscriptionTag>> GetAllForTagAsync(int tagId, CancellationToken cancellationToken = default);

    Task DeleteAllForFeedSubscriptionAsync(int feedSubscriptionId, CancellationToken cancellationToken = default);
}
