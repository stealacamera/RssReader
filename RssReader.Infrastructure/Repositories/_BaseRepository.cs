using Microsoft.EntityFrameworkCore;
using RssReader.Application.Abstractions.Repositories;
using RssReader.Domain.Common;
using System.Linq.Expressions;

namespace RssReader.Infrastructure.Repositories;

internal abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly DbSet<TEntity> _set;
    protected readonly IQueryable<TEntity> _untrackedSet;

    protected BaseRepository(AppDbContext dbContext)
    {
        _set = dbContext.Set<TEntity>();
        _untrackedSet = _set.AsNoTracking();
    }

    protected static async Task<PaginatedEnumerable<TCompareProperty, TEntity>> GetCursorPaginationAsync<TCompareProperty>(
        IQueryable<TEntity> query,
        Expression<Func<TEntity, TCompareProperty>> comparisonPropertySelector,
        TCompareProperty? cursor,
        int pageSize,
        bool getNewerValues = true,
        CancellationToken cancellationToken = default)
        where TCompareProperty : struct, IComparable<TCompareProperty>
    {
        if (cursor.HasValue)
            query = query.Where(GenerateComparisonLambda(
                                    comparisonPropertySelector, 
                                    cursor.Value, 
                                    getNewerValues));

        query = getNewerValues ?
                query.OrderBy(comparisonPropertySelector) :
                query.OrderByDescending(comparisonPropertySelector);

        var values = await query.Take(pageSize + 1)
                                .ToListAsync(cancellationToken);

        TCompareProperty? newCursor = null;

        // Get cursor value if there are any entities left in pagination
        if (values.Skip(1).Any() && values.Count >= pageSize)
        {
            newCursor = comparisonPropertySelector.Compile()(values[^1]);
            values.RemoveAt(values.Count - 1);
        }

        return new PaginatedEnumerable<TCompareProperty, TEntity>
        {
            NextCursor = newCursor,
            Values = values
        };
    }

    private static Expression<Func<TEntity, bool>> GenerateComparisonLambda<TCompareProperty>(
        Expression<Func<TEntity, TCompareProperty>> comparisonPropertySelector,
        TCompareProperty cursor,
        bool getNewerValues)
        where TCompareProperty : struct, IComparable<TCompareProperty>
    {
        var comparisonProperty = comparisonPropertySelector.Parameters.First();

        Expression<Func<TCompareProperty, bool>> compareToCursorFunc = getNewerValues ?
                                                       x => x.CompareTo(cursor) >= 0 :
                                                       x => x.CompareTo(cursor) <= 0;

        return Expression.Lambda<Func<TEntity, bool>>(
                                    Expression.Invoke(
                                        compareToCursorFunc,
                                        comparisonPropertySelector.Body),
                                    comparisonProperty);
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => await _set.AddAsync(entity, cancellationToken);

    public void Delete(TEntity entity)
        => _set.Remove(entity);
}
