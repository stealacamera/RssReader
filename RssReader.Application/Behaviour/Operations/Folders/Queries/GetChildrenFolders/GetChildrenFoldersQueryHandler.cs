using FluentValidation;
using MediatR;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions.General;

namespace RssReader.Application.Behaviour.Operations.Folders.Queries.GetChildrenFolders;

internal class GetChildrenFoldersQueryHandler : BaseHandler, IRequestHandler<GetChildrenFoldersQuery, IList<Folder>>
{
    public GetChildrenFoldersQueryHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task<IList<Folder>> Handle(GetChildrenFoldersQuery request, CancellationToken cancellationToken)
    {
        await ValidateRequestAsync(request, cancellationToken);

        var childrenFolders = await _workUnit.FoldersRepository
                                             .GetAllChildrenForFolderAsync(request.FolderId, cancellationToken);

        return childrenFolders.Select(e => new Folder(e.Id, e.Name, e.OwnerId))
                              .ToList();
    }

    private async Task ValidateRequestAsync(GetChildrenFoldersQuery request, CancellationToken cancellationToken)
    {
        await new GetChildrenFoldersQueryValidator().ValidateAndThrowAsync(request, cancellationToken);
        await ValidateRequesterAsync(request.RequesterId, cancellationToken);

        var parentFolder = await _workUnit.FoldersRepository
                                          .GetByIdAsync(request.FolderId, cancellationToken);

        // Validate folder & ownership
        if (parentFolder == null)
            throw new EntityNotFoundException(nameof(Folder));
        else if (parentFolder.OwnerId != request.RequesterId)
            throw new UnauthorizedException();
    }
}
