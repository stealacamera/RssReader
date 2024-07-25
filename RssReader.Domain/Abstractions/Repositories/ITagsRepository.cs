using RssReader.Domain.Entities;

namespace RssReader.Domain.Abstractions.Repositories;

public interface ITagsRepository : IBaseSimpleRepository<Tag>
{
    Task<IEnumerable<Tag>> GetAllForUserAsync(int requesterId, CancellationToken? cancellationToken = null);
    Task<Tag> GetByNameForUserAsync(string name, int requesterId, CancellationToken? cancellationToken = null);
}
