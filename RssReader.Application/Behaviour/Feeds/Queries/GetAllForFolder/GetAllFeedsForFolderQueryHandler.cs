using FluentValidation;
using MediatR;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions;
using RssReader.Domain.Abstractions;

namespace RssReader.Application.Behaviour.Feeds.Queries.GetAllForFolder;

internal class GetAllFeedsForFolderQueryHandler : BaseCommandHandler, IRequestHandler<GetAllFeedsForFolderQuery, IList<Feed>>
{
    public GetAllFeedsForFolderQueryHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task<IList<Feed>> Handle(GetAllFeedsForFolderQuery request, CancellationToken cancellationToken)
    {
        // Validate request
        await ValidateRequestAsync(request, cancellationToken);

        // Retrieve folder feeds
        var feeds = await _workUnit.FeedsRepository.GetAllForFolderAsync(request.FolderId, cancellationToken);

        return feeds.Select(e => new Feed(e.Id, e.Url, e.Name))
                    .ToList();
    }

    private async Task ValidateRequestAsync(GetAllFeedsForFolderQuery request, CancellationToken cancellationToken)
    {
        // Validate request properties
        await new GetAllFeedsForFolderQueryValidator().ValidateAndThrowAsync(request, cancellationToken);

        // Validate user
        if (!await _workUnit.UsersRepository.DoesInstanceExistAsync(request.RequesterId, cancellationToken))
            throw new EntityNotFoundException(nameof(User));

        // Validate folder & ownership
        var folder = await _workUnit.FoldersRepository
                                    .GetByIdAsync(request.FolderId, cancellationToken);

        if (folder == null)
            throw new EntityNotFoundException(nameof(Folder));
        else if (folder.OwnerId != request.RequesterId)
            throw new UnauthorizedException();
    }
}
