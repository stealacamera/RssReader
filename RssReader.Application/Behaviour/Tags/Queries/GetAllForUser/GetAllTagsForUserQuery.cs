using FluentValidation;
using MediatR;
using RssReader.Application.Common.DTOs;

namespace RssReader.Application.Behaviour.Tags.Queries.GetAllForUser;

public record GetAllTagsForUserQuery(int RequesterId) : IRequest<IList<Tag>>;

internal class GetAllTagsForUserQueryValidator : AbstractValidator<GetAllTagsForUserQuery>
{
    public GetAllTagsForUserQueryValidator()
    {
        RuleFor(e => e.RequesterId)
            .NotEmpty()
            .GreaterThan(0);
    }
}