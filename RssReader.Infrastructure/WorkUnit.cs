using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using RssReader.Application.Abstractions;
using RssReader.Application.Abstractions.Repositories;
using RssReader.Infrastructure.Repositories;

namespace RssReader.Infrastructure;

internal class WorkUnit : IWorkUnit
{
    private readonly AppDbContext _dbContext;

    public WorkUnit(IServiceProvider serviceProvider)
        => _dbContext = serviceProvider.GetRequiredService<AppDbContext>();

    private IUsersRepository _usersRepository = null!;

    public IUsersRepository UsersRepository
    {
        get
        {
            _usersRepository ??= new UsersRepository(_dbContext);
            return _usersRepository;
        }
    }

    private IFoldersRepository _foldersRepository = null!;
    public IFoldersRepository FoldersRepository 
    {
        get
        {
            _foldersRepository ??= new FoldersRepository(_dbContext);
            return _foldersRepository;
        }
    }

    private IFeedsRepository _feedsRepository = null!;
    public IFeedsRepository FeedsRepository
    {
        get
        {
            _feedsRepository ??= new FeedsRepository(_dbContext);
            return _feedsRepository;
        }
    }

    private ITagsRepository _tagsRepository = null!;
    public ITagsRepository TagsRepository
    {
        get
        {
            _tagsRepository ??= new TagsRepository(_dbContext);
            return _tagsRepository;
        }
    }

    private IFeedTagsRepository _feedTagsRepository = null!;
    public IFeedTagsRepository FeedTagsRepository
    {
        get
        {
            _feedTagsRepository ??= new FeedTagsRepository(_dbContext);
            return _feedTagsRepository;
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

    public async Task<IDbContextTransaction> BeginTransactionAsync()
        => await _dbContext.Database.BeginTransactionAsync();

    public async Task SaveChangesAsync()
        => await _dbContext.SaveChangesAsync();
}
