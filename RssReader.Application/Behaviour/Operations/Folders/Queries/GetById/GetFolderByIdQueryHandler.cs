using FluentValidation;
using MediatR;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions.General;

namespace RssReader.Application.Behaviour.Operations.Folders.Queries.GetById;

internal class GetFolderByIdQueryHandler : BaseHandler, IRequestHandler<GetFolderByIdQuery, Folder>
{
    public GetFolderByIdQueryHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task<Folder> Handle(GetFolderByIdQuery request, CancellationToken cancellationToken)
    {
        var folder = await ValidateRequestAsync(request, cancellationToken);

        // Retrieve folder's feed subscriptions
        var feedSubscriptions = (await _workUnit.FeedSubscriptionsRepository
                                               .GetAllForFolderAsync(request.FolderId, cancellationToken))
                                               .Select(async e => await GetFeedSubscriptionAsync(e, cancellationToken))
                                               .Select(e => e.Result)
                                               .ToList();

        // Retrieve folder's subfolders
        var subfolders = (await _workUnit.FoldersRepository
                                         .GetAllChildrenForFolderAsync(request.FolderId, cancellationToken))
                                         .Select(e => new SimpleFolder(e.Id, e.Name))
                                         .ToList();

        return new Folder(folder.Id, folder.Name, Subfolders: subfolders, Feeds: feedSubscriptions);
    }

    private async Task<Domain.Entities.Folder> ValidateRequestAsync(GetFolderByIdQuery request, CancellationToken cancellationToken)
    {
        await new GetFolderByIdQueryValidator().ValidateAndThrowAsync(request, cancellationToken);
        await ValidateRequesterAsync(request.RequesterId, cancellationToken);

        var folder = await _workUnit.FoldersRepository
                                    .GetByIdAsync(request.FolderId, cancellationToken);

        if (folder == null)
            throw new EntityNotFoundException(nameof(Folder));
        else if (folder.OwnerId != request.RequesterId)
            throw new UnauthorizedException();

        return folder;
    }

    private async Task<FeedSubscription> GetFeedSubscriptionAsync(Domain.Entities.FeedSubscription entity, CancellationToken cancellationToken)
    {
        var subscriptionTags = await _workUnit.FeedSubscriptionTagsRepository
                                              .GetAllForFeedSubscriptionAsync(entity.Id, cancellationToken);

        IList<Tag> tags = new List<Tag>();

        foreach (var tag in subscriptionTags)
        {
            var tagEntity = (await _workUnit.TagsRepository
                                            .GetByIdAsync(tag.TagId, cancellationToken))!;

            tags.Add(new Tag(tagEntity.Id, tagEntity.Name));
        }

        return new FeedSubscription(entity.Id, entity.Name, tags);
    }
}
