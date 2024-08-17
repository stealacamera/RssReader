using MediatR;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions;

namespace RssReader.Application.Behaviour.Operations.Folders.Queries.GetAllForUser;

internal class GetAllFoldersForUserQueryHandler : BaseHandler, IRequestHandler<GetAllFoldersForUserQuery, IList<Folder>>
{
    public GetAllFoldersForUserQueryHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task<IList<Folder>> Handle(GetAllFoldersForUserQuery request, CancellationToken cancellationToken)
    {
        // Validate request
        var validationDetails = await new GetAllFoldersForUserQueryValidator().ValidateAsync(request, cancellationToken);

        if (!validationDetails.IsValid)
            throw new ValidationException(validationDetails.ToDictionary());

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
