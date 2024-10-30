using FluentValidation;
using MediatR;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions.General;

namespace RssReader.Application.Behaviour.Operations.Folders.Commands.Delete;

internal class DeleteFolderCommandHandler : BaseHandler, IRequestHandler<DeleteFolderCommand>
{
    public DeleteFolderCommandHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task Handle(DeleteFolderCommand request, CancellationToken cancellationToken)
    {
        await new DeleteFolderCommandValidator().ValidateAndThrowAsync(request, cancellationToken);
        await ValidateRequesterAsync(request.RequesterId, cancellationToken);

        var folder = await _workUnit.FoldersRepository
                                    .GetByIdAsync(request.FolderId);

        if (folder == null)
            throw new EntityNotFoundException(nameof(Folder));
        else if (folder.OwnerId != request.RequesterId)
            throw new UnauthorizedException();

        await WrapInTransactionAsync(async () => await DeleteFolderAsync(folder, cancellationToken));
    }

    private async Task DeleteFolderAsync(Domain.Entities.Folder folder, CancellationToken cancellationToken)
    {
        var feedSubscriptions = await _workUnit.FeedSubscriptionsRepository
                                              .GetAllForFolderAsync(folder.Id, cancellationToken);

        // Delete user tags
        foreach(var subscription in feedSubscriptions)
            await _workUnit.FeedSubscriptionTagsRepository
                           .DeleteAllForFeedSubscriptionAsync(subscription.Id, cancellationToken);

        await _workUnit.SaveChangesAsync();

        // Delete subscriptions in folder
        await _workUnit.FeedSubscriptionsRepository.DeleteAllForFolderAsync(folder.Id, cancellationToken);
        await _workUnit.SaveChangesAsync();

        // Repeat for each subfolder recursively
        var subFolders = await _workUnit.FoldersRepository
                                        .GetAllChildrenForFolderAsync(folder.Id, cancellationToken);

        foreach (var subFolder in subFolders)
        {
            await DeleteFolderAsync(subFolder, cancellationToken);
            await _workUnit.SaveChangesAsync();
        }

        // Delete current folder
        _workUnit.FoldersRepository.Delete(folder);
        await _workUnit.SaveChangesAsync();
    }
}
