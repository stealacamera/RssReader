using FluentValidation;
using MediatR;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions.General;

namespace RssReader.Application.Behaviour.Operations.Folders.Commands.Create;

internal class CreateFolderCommandHandler : BaseHandler, IRequestHandler<CreateFolderCommand, Folder>
{
    public CreateFolderCommandHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task<Folder> Handle(CreateFolderCommand request, CancellationToken cancellationToken)
    {
        await ValidateRequestAsync(request, cancellationToken);

        var newFolder = new Domain.Entities.Folder
        {
            Name = request.FolderName.Trim(),
            OwnerId = request.RequesterId,
            ParentId = request.ParentFolderId,
            CreatedAt = DateTime.UtcNow,
        };

        await _workUnit.FoldersRepository.AddAsync(newFolder, cancellationToken);
        await _workUnit.SaveChangesAsync();

        return new Folder(newFolder.Id, newFolder.Name);
    }

    private async Task ValidateRequestAsync(CreateFolderCommand request, CancellationToken cancellationToken)
    {
        await new CreateFolderCommandValidator().ValidateAndThrowAsync(request, cancellationToken);
        await ValidateRequesterAsync(request.RequesterId, cancellationToken);

        // Validate parent folder (if given) & ownership
        if (request.ParentFolderId.HasValue)
        {
            var parentFolder = await _workUnit.FoldersRepository
                                              .GetByIdAsync(request.ParentFolderId.Value, cancellationToken);

            if (parentFolder == null)
                throw new EntityNotFoundException(nameof(Folder));
            else if (parentFolder.OwnerId != request.RequesterId)
                throw new UnauthorizedException();
        }
    }
}
