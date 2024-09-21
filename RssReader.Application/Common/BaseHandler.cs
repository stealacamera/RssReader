using RssReader.Application.Abstractions;
using RssReader.Application.Common.Exceptions.General;

namespace RssReader.Application.Common;

internal class BaseHandler
{
    protected static readonly SemaphoreSlim _asyncLock = new SemaphoreSlim(1, 1);
    protected IWorkUnit _workUnit;

    public BaseHandler(IWorkUnit workUnit) => _workUnit = workUnit;

    protected async Task<T> WrapInTransactionAsync<T>(Func<Task<T>> asyncFunc)
    {
        await _asyncLock.WaitAsync();

        using var transaction = await _workUnit.BeginTransactionAsync();
        T result;

        try
        {
            result = await asyncFunc();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
        finally
        {
            _asyncLock.Release();
        }

        return result;
    }

    protected async Task ValidateRequesterAsync(int requesterId, CancellationToken cancellationToken)
    {
        if (!await _workUnit.UsersRepository
                           .DoesInstanceExistAsync(requesterId, cancellationToken))
            throw new UnauthorizedException();
    }
}
