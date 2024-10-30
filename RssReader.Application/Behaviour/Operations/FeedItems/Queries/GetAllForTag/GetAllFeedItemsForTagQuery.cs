using FluentValidation;
using MediatR;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Validation;

namespace RssReader.Application.Behaviour.Operations.FeedItems.Queries.GetAllForTag;

public record GetAllFeedItemsForTagQuery(
    int RequesterId, 
    int TagId, 
    int PageSize,
    DateTime? Cursor) 
    : IRequest<PaginatedResponse<DateTime, IList<FeedItem>>>;

internal class GetAllFeedItemsForTagQueryValidator : Validator<GetAllFeedItemsForTagQuery>
{
    public GetAllFeedItemsForTagQueryValidator()
    {
        RuleFor(e => e.RequesterId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(e => e.TagId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(e => e.PageSize)
            .NotEmpty()
            .GreaterThan(0);
    }
}