using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using RssReader.Application.Abstractions;
using RssReader.Application.Abstractions.Repositories;
using RssReader.Application.Abstractions.Repositories.Identity;
using RssReader.Infrastructure.Repositories;
using RssReader.Infrastructure.Repositories.Identity;

namespace RssReader.Infrastructure;

internal class WorkUnit : IWorkUnit
{
    private readonly AppDbContext _dbContext;
    private readonly IDistributedCache _distributedCache;

    public WorkUnit(IServiceProvider serviceProvider)
    {
        _dbContext = serviceProvider.GetRequiredService<AppDbContext>();
        _distributedCache = serviceProvider.GetRequiredService<IDistributedCache>();
    }
    
    public async Task<IDbContextTransaction> BeginTransactionAsync()
        => await _dbContext.Database.BeginTransactionAsync();

    public async Task SaveChangesAsync()
        => await _dbContext.SaveChangesAsync();

    #region Identity
    private IUsersRepository _usersRepository = null!;
    public IUsersRepository UsersRepository
    {
        get
        {
            _usersRepository ??= new UsersRepository(_dbContext);
            return _usersRepository;
        }
    }

    private IUserRolesRepository _userRolesRepository = null!;
    public IUserRolesRepository UserRolesRepository
    {
        get
        {
            _userRolesRepository ??= new UserRolesRepository(_dbContext, _distributedCache);
            return _userRolesRepository;
        }
    }

    private IRolePermissionsRepository _rolePermissionsRepository = null!;
    public IRolePermissionsRepository RolePermissionsRepository
    {
        get
        {
            _rolePermissionsRepository ??= new RolePermissionsRepository(_dbContext);
            return _rolePermissionsRepository;
        }
    }

    private IOTPsRepository _otpsRepository = null!;
    public IOTPsRepository OTPsRepository
    {
        get
        {
            _otpsRepository ??= new OTPsRepository(_dbContext);
            return _otpsRepository;
        }
    }
    #endregion

    private IFoldersRepository _foldersRepository = null!;
    public IFoldersRepository FoldersRepository 
    {
        get
        {
            _foldersRepository ??= new FoldersRepository(_dbContext);
            return _foldersRepository;
        }
    }

    #region Feeds
    private IFeedsRepository _feedsRepository = null!;
    public IFeedsRepository FeedsRepository
    {
        get
        {
            _feedsRepository ??= new FeedsRepository(_dbContext);
            return _feedsRepository;
        }
    }

    private IFeedItemsRepository _feedItemsRepository = null!;
    public IFeedItemsRepository FeedItemsRepository
    {
        get
        {
            _feedItemsRepository ??= new FeedItemsRepository(_dbContext);
            return _feedItemsRepository;
        }
    }
    
    private IFeedSubscriptionsRepository _feedSubscriptionsRepository = null!;
    public IFeedSubscriptionsRepository FeedSubscriptionsRepository
    {
        get
        {
            _feedSubscriptionsRepository ??= new FeedSubscriptionsRepository(_dbContext);
            return _feedSubscriptionsRepository;
        }
    }
    #endregion

    private ITagsRepository _tagsRepository = null!;
    public ITagsRepository TagsRepository
    {
        get
        {
            _tagsRepository ??= new TagsRepository(_dbContext);
            return _tagsRepository;
        }
    }

    private IFeedSubscriptionTagsRepository _feedTagsRepository = null!;
    public IFeedSubscriptionTagsRepository FeedSubscriptionTagsRepository
    {
        get
        {
            _feedTagsRepository ??= new FeedSubscriptionTagsRepository(_dbContext);
            return _feedTagsRepository;
        }
    }
}
