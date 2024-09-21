using FluentValidation;
using MediatR;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;

namespace RssReader.Application.Behaviour.Operations.Folders.Queries.GetAllForUser;

internal class GetAllFoldersForUserQueryHandler : BaseHandler, IRequestHandler<GetAllFoldersForUserQuery, IList<Folder>>
{
    public GetAllFoldersForUserQueryHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task<IList<Folder>> Handle(GetAllFoldersForUserQuery request, CancellationToken cancellationToken)
    {
        // Validate request
        await new GetAllFoldersForUserQueryValidator().ValidateAndThrowAsync(request, cancellationToken);
        await ValidateRequesterAsync(request.RequesterId, cancellationToken);

        // Retrieve folders
        var userFolders = await _workUnit.FoldersRepository
                                         .GetAllForUserAsync(request.RequesterId, cancellationToken);

        return userFolders.Select(e => new Folder(e.Id, e.Name, request.RequesterId))
                          .ToList();
    }
}
