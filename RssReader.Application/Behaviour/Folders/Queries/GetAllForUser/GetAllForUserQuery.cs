using FluentValidation;
using MediatR;
using RssReader.Application.Common.DTOs;

namespace RssReader.Application.Behaviour.Folders.Queries.GetAllForUser;

public record GetAllForUserQuery(int UserId) : IRequest<IList<Folder>>;

internal class GetAllForUserQueryValidator : AbstractValidator<GetAllForUserQuery>
{
    public GetAllForUserQueryValidator()
    {
        RuleFor(e => e.UserId)
            .NotEmpty()
            .GreaterThan(0);
    }
}