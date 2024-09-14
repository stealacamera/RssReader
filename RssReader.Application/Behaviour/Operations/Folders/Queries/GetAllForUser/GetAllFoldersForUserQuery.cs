using FluentValidation;
using MediatR;
using RssReader.Application.Common.DTOs;

namespace RssReader.Application.Behaviour.Operations.Folders.Queries.GetAllForUser;

public record GetAllFoldersForUserQuery(int RequesterId, int UserId) : IRequest<IList<Folder>>;

internal class GetAllFoldersForUserQueryValidator : AbstractValidator<GetAllFoldersForUserQuery>
{
    public GetAllFoldersForUserQueryValidator()
    {
        RuleFor(e => e.RequesterId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(e => e.UserId)
            .NotEmpty()
            .GreaterThan(0);
    }
}