using FluentValidation;
using MediatR;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Validation;

namespace RssReader.Application.Behaviour.Operations.FeedItems.Queries.GetAllForFolder;

public record GetAllFeedItemsForFolderQuery(
    int RequesterId, 
    int FolderId, 
    int PageSize, 
    DateTime? Cursor) 
    : IRequest<PaginatedResponse<DateTime, IList<FeedItem>>>;

internal class GetAllFeedItemsForFolderQueryValidator : Validator<GetAllFeedItemsForFolderQuery>
{
    public GetAllFeedItemsForFolderQueryValidator()
    {
        RuleFor(e => e.RequesterId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(e => e.FolderId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(e => e.PageSize)
            .NotEmpty()
            .GreaterThan(0);
    }
}