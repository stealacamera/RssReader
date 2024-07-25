using FluentValidation;
using MediatR;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions;
using RssReader.Domain.Abstractions;

namespace RssReader.Application.Behaviour.Folders.Queries.GetChildrenFolders;

internal class GetChildrenFoldersQueryHandler : BaseCommandHandler, IRequestHandler<GetChildrenFoldersQuery, IList<Folder>>
{
    public GetChildrenFoldersQueryHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task<IList<Folder>> Handle(GetChildrenFoldersQuery request, CancellationToken cancellationToken)
    {
        await ValidateRequestAsync(request, cancellationToken);

        var parentFolder = await _workUnit.FoldersRepository
                                          .GetByIdAsync(request.FolderId, cancellationToken);

        var childrenFolders = await _workUnit.FoldersRepository
                                             .GetAllChildrenForFolderAsync(parentFolder.Id, cancellationToken);

        return childrenFolders.Select(e => new Folder(e.Id, e.Name, e.OwnerId))
                              .ToList();
    }

    private async Task ValidateRequestAsync(GetChildrenFoldersQuery request, CancellationToken cancellationToken)
    {
        // validate request
        await new GetChildrenFoldersQueryValidator().ValidateAndThrowAsync(request, cancellationToken);

        var parentFolder = await _workUnit.FoldersRepository
                                          .GetByIdAsync(request.FolderId, cancellationToken);

        // Validate folder
        if (!await _workUnit.FoldersRepository.DoesInstanceExistAsync(request.FolderId))
            throw new EntityNotFoundException(nameof(Folder));

        // Validate requester as owner
        if (!await _workUnit.UsersRepository.DoesInstanceExistAsync(request.RequesterId))
            throw new EntityNotFoundException(nameof(User));
        else if (parentFolder.OwnerId != request.RequesterId)
            throw new UnauthorizedException();
    }
}
