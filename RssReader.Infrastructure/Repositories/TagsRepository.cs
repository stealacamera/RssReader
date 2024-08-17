﻿using Microsoft.EntityFrameworkCore;
using RssReader.Application.Abstractions.Repositories;
using RssReader.Domain.Entities;

namespace RssReader.Infrastructure.Repositories;

internal class TagsRepository : BaseSimpleRepository<Tag>, ITagsRepository
{
    public TagsRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Tag>> GetAllForUserAsync(int requesterId, CancellationToken cancellationToken = default)
    {
        var query = _set.Where(e => e.OwnerId == requesterId);
        return await query.ToListAsync(cancellationToken);
    }
}