namespace RssReader.Domain.Common;

public class PaginatedEnumerable<TCursor, TEntity>
    where TCursor : struct, IComparable<TCursor>
{
    public TCursor? NextCursor { get; set; }
    public IEnumerable<TEntity> Values { get; set; } = [];
}
