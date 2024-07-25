using Microsoft.EntityFrameworkCore.Storage;
using RssReader.Domain.Abstractions.Repositories;

namespace RssReader.Domain.Abstractions;

public interface IWorkUnit
{
    Task<IDbContextTransaction> BeginTransactionAsync();

    IUsersRepository UsersRepository { get; }
    IFoldersRepository FoldersRepository { get; }
}
