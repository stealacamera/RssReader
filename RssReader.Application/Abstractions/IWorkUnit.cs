using Microsoft.EntityFrameworkCore.Storage;
using RssReader.Application.Abstractions.Repositories;
using RssReader.Application.Abstractions.Repositories.Identity;

namespace RssReader.Application.Abstractions;

public interface IWorkUnit
{
    Task SaveChangesAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();

    IUsersRepository UsersRepository { get; }
    IUserRolesRepository UserRolesRepository { get; }
    IRolePermissionsRepository RolePermissionsRepository { get; }
    IOTPsRepository OTPsRepository { get; }

    #region Feeds
    IFeedsRepository FeedsRepository { get; }
    IFeedItemsRepository FeedItemsRepository { get; }
    IFeedSubscriptionsRepository FeedSubscriptionsRepository { get; }
    #endregion

    IFoldersRepository FoldersRepository { get; }
    ITagsRepository TagsRepository { get; }
    IFeedSubscriptionTagsRepository FeedSubscriptionTagsRepository { get; }
}
