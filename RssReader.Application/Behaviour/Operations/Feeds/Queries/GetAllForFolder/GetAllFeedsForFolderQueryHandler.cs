using FluentValidation;
using MediatR;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions.General;

namespace RssReader.Application.Behaviour.Operations.Feeds.Queries.GetAllForFolder;

internal class GetAllFeedsForFolderQueryHandler : BaseHandler, IRequestHandler<GetAllFeedsForFolderQuery, IList<Feed>>
{
    public GetAllFeedsForFolderQueryHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task<IList<Feed>> Handle(GetAllFeedsForFolderQuery request, CancellationToken cancellationToken)
    {
        // Validate request
        await ValidateRequestAsync(request, cancellationToken);

        // Retrieve folder feeds
        var feeds = await _workUnit.FeedsRepository
                                   .GetAllForFolderAsync(request.FolderId, cancellationToken);

        return feeds.Select(e => new Feed(e.Id, e.Url, e.Name))
                    .ToList();
    }

    private async Task ValidateRequestAsync(GetAllFeedsForFolderQuery request, CancellationToken cancellationToken)
    {
        // Validate request properties
        await new GetAllFeedsForFolderQueryValidator().ValidateAndThrowAsync(request, cancellationToken);
        await ValidateRequesterAsync(request.RequesterId, cancellationToken);

        // Validate folder & ownership
        var folder = await _workUnit.FoldersRepository
                                    .GetByIdAsync(request.FolderId, cancellationToken);

        if (folder == null)
            throw new EntityNotFoundException(nameof(Folder));
        else if (folder.OwnerId != request.RequesterId)
            throw new UnauthorizedException();
    }
}
