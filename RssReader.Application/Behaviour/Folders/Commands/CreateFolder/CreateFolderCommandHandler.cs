using FluentValidation;
using MediatR;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions;
using RssReader.Domain.Abstractions;
using System.Threading;

namespace RssReader.Application.Behaviour.Folders.Commands.CreateFolder;

internal class CreateFolderCommandHandler : BaseCommandHandler, IRequestHandler<CreateFolderCommand, Folder>
{
    public CreateFolderCommandHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task<Folder> Handle(CreateFolderCommand request, CancellationToken cancellationToken)
    {
        await ValidateRequestAsync(request, cancellationToken);

        var newFolder = await _workUnit.FoldersRepository
                                       .AddAsync(new Domain.Entities.Folder
                                       {
                                           Name = request.FolderName,
                                           OwnerId = request.RequesterId,
                                           ParentId = request.ParentFolderId
                                       });

        return new Folder(newFolder.Id, newFolder.Name, newFolder.OwnerId);
    }

    private async Task ValidateRequestAsync(CreateFolderCommand request, CancellationToken cancellationToken)
    {
        // Validate properties
        await new CreateFolderCommandValidator().ValidateAndThrowAsync(request, cancellationToken);

        // Validate requester
        if (!await _workUnit.UsersRepository.DoesInstanceExistAsync(request.RequesterId, cancellationToken))
            throw new EntityNotFoundException(nameof(User));

        // Validate parent folder (if given)
        if (request.ParentFolderId.HasValue && 
            !await _workUnit.FoldersRepository.DoesInstanceExistAsync(request.ParentFolderId.Value))
            throw new EntityNotFoundException(nameof(Folder));

        // Validate new folder
        if (await _workUnit.FoldersRepository.GetByNameAsync(request.RequesterId, request.FolderName, cancellationToken) != null)
            ; // return specific error for name property
    }
}
