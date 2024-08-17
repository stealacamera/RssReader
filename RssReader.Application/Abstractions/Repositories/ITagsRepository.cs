using RssReader.Domain.Entities;

namespace RssReader.Application.Abstractions.Repositories;

public interface ITagsRepository : IBaseSimpleRepository<Tag>
{
    Task<IEnumerable<Tag>> GetAllForUserAsync(int requesterId, CancellationToken cancellationToken = default);
}
