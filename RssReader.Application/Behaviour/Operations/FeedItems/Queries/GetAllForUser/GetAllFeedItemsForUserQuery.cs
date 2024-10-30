using FluentValidation;
using MediatR;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Validation;

namespace RssReader.Application.Behaviour.Operations.FeedItems.Queries.GetAllForUser;

public record GetAllFeedItemsForUserQuery(
    int RequesterId, 
    int PageSize, 
    DateTime? Cursor) 
    : IRequest<PaginatedResponse<DateTime, IList<FeedItem>>>;

internal class GetAllFeedItemsForUserQueryValidator : Validator<GetAllFeedItemsForUserQuery>
{
    public GetAllFeedItemsForUserQueryValidator()
    {
        RuleFor(e => e.RequesterId)
            .NotEmpty()
            .GreaterThan(0);
    }
}