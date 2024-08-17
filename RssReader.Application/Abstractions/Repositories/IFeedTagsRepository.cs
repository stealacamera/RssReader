using RssReader.Domain.Entities;

namespace RssReader.Application.Abstractions.Repositories;

public interface IFeedTagsRepository : IBaseRepository<FeedTag>
{
    Task<FeedTag?> GetByIdsAsync(int feedId, int tagId, CancellationToken cancellationToken = default);
    Task<bool> DoesInstanceExistAsync(int feedId, int tagId, CancellationToken cancellationToken = default);
}
