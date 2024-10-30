using FluentValidation;
using MediatR;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Validation;

namespace RssReader.Application.Behaviour.Operations.Folders.Queries.GetById;

public record GetFolderByIdQuery(int RequesterId, int FolderId) : IRequest<Folder>;

internal class GetFolderByIdQueryValidator : Validator<GetFolderByIdQuery>
{
    public GetFolderByIdQueryValidator()
    {
        RuleFor(e => e.RequesterId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(e => e.FolderId)
            .NotEmpty()
            .GreaterThan(0);
    }
}