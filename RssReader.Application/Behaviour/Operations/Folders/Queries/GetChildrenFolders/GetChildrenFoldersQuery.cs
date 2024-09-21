using FluentValidation;
using MediatR;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Validation;

namespace RssReader.Application.Behaviour.Operations.Folders.Queries.GetChildrenFolders;

public record GetChildrenFoldersQuery(int RequesterId, int FolderId) : IRequest<IList<Folder>>;

internal class GetChildrenFoldersQueryValidator : Validator<GetChildrenFoldersQuery>
{
    public GetChildrenFoldersQueryValidator()
    {
        RuleFor(e => e.RequesterId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(e => e.FolderId)
            .NotEmpty()
            .GreaterThan(0);
    }
}