using Microsoft.EntityFrameworkCore.Storage;
using RssReader.Application.Abstractions.Repositories;

namespace RssReader.Application.Abstractions;

public interface IWorkUnit
{
    Task SaveChangesAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();

    IUsersRepository UsersRepository { get; }
    IFoldersRepository FoldersRepository { get; }
    IFeedsRepository FeedsRepository { get; }
    ITagsRepository TagsRepository { get; }
    IFeedTagsRepository FeedTagsRepository { get; }
    IOTPsRepository OTPsRepository { get; }
}
