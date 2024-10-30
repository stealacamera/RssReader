using FluentValidation;
using MediatR;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Validation;

namespace RssReader.Application.Behaviour.Operations.FeedItems.Queries.GetAllForFeed;

public record GetAllFeedItemsForFeedQuery(
    int RequesterId, 
    int FeedSubscriptionId, 
    int PageSize, 
    DateTime? Cursor) 
    : IRequest<PaginatedResponse<DateTime, IList<FeedItem>>>;

internal class GetAllFeedItemsForFeedQueryValidator : Validator<GetAllFeedItemsForFeedQuery>
{
    public GetAllFeedItemsForFeedQueryValidator()
    {
        RuleFor(e => e.RequesterId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(e => e.FeedSubscriptionId)
            .NotEmpty()
            .GreaterThan(0);
    }
}