using FluentValidation;
using MediatR;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Validation;

namespace RssReader.Application.Behaviour.Operations.Tags.Queries.GetAllForUser;

public record GetAllTagsForUserQuery(int RequesterId) : IRequest<IList<Tag>>;

internal class GetAllTagsForUserQueryValidator : Validator<GetAllTagsForUserQuery>
{
    public GetAllTagsForUserQueryValidator()
    {
        RuleFor(e => e.RequesterId)
            .NotEmpty()
            .GreaterThan(0);
    }
}