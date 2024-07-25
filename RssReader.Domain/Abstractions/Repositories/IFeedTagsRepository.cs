using RssReader.Domain.Entities;

namespace RssReader.Domain.Abstractions.Repositories;

public interface IFeedTagsRepository : IBaseRepository<FeedTag>
{
    Task<FeedTag?> GetByIdsAsync(int feedId, int tagId, CancellationToken? cancellationToken = null);
    Task<bool> DoesInstanceExistAsync(int feedId, int tagId, CancellationToken? cancellationToken = null);

    Task<FeedTag> CreateAsync(FeedTag feedTag, CancellationToken? cancellationToken = null);
}
