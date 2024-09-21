using FluentValidation;
using MediatR;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Validation;

namespace RssReader.Application.Behaviour.Operations.Folders.Queries.GetAllForUser;

public record GetAllFoldersForUserQuery(int RequesterId) : IRequest<IList<Folder>>;

internal class GetAllFoldersForUserQueryValidator : Validator<GetAllFoldersForUserQuery>
{
    public GetAllFoldersForUserQueryValidator()
    {
        RuleFor(e => e.RequesterId)
            .NotEmpty()
            .GreaterThan(0);
    }
}