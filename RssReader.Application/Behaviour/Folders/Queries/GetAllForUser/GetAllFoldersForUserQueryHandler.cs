using FluentValidation;
using MediatR;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions;
using RssReader.Domain.Abstractions;

namespace RssReader.Application.Behaviour.Folders.Queries.GetAllForUser;

internal class GetAllFoldersForUserQueryHandler : BaseCommandHandler, IRequestHandler<GetAllFoldersForUserQuery, IList<Folder>>
{
    public GetAllFoldersForUserQueryHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task<IList<Folder>> Handle(GetAllFoldersForUserQuery request, CancellationToken cancellationToken)
    {
        // Validate request
        await new GetAllFoldersForUserQueryValidator().ValidateAndThrowAsync(request, cancellationToken);

        // Validate user
        if (!await _workUnit.UsersRepository.DoesInstanceExistAsync(request.UserId, cancellationToken))
            throw new EntityNotFoundException(nameof(User));

        // Retrieve folders
        var userFolders = await _workUnit.FoldersRepository
                                         .GetAllForUserAsync(request.UserId, cancellationToken);

        return userFolders.Select(e => new Folder(e.Id, e.Name, request.UserId))
                          .ToList();
    }
}
