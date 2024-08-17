using RssReader.Application.Abstractions.Repositories;
using RssReader.Domain.Entities;

namespace RssReader.Infrastructure.Repositories;

internal class FeedTagsRepository : BaseRepository<FeedTag>, IFeedTagsRepository
{
    public FeedTagsRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> DoesInstanceExistAsync(int feedId, int tagId, CancellationToken cancellationToken = default)
        => await _set.FindAsync([feedId, tagId], cancellationToken: cancellationToken) != null;

    public async Task<FeedTag?> GetByIdsAsync(int feedId, int tagId, CancellationToken cancellationToken = default)
        => await _set.FindAsync([feedId, tagId], cancellationToken: cancellationToken);
}
