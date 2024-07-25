using Microsoft.EntityFrameworkCore.Storage;
using RssReader.Domain.Abstractions.Repositories;

namespace RssReader.Domain.Abstractions;

public interface IWorkUnit
{
    Task SaveChangesAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();

    IUsersRepository UsersRepository { get; }
    IFoldersRepository FoldersRepository { get; }
    IFeedsRepository FeedsRepository { get; }
    ITagsRepository TagsRepository { get; }
    IFeedTagsRepository FeedTagsRepository { get; }
}
