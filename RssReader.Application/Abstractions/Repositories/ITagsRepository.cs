using RssReader.Domain.Entities;

namespace RssReader.Application.Abstractions.Repositories;

public interface ITagsRepository : IBaseSimpleRepository<int, Tag>
{
    Task<IEnumerable<Tag>> GetAllForUserAsync(int requesterId, CancellationToken cancellationToken = default);
}
