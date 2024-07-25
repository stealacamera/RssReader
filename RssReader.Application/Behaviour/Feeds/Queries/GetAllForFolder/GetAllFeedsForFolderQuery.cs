using FluentValidation;
using MediatR;
using RssReader.Application.Common.DTOs;

namespace RssReader.Application.Behaviour.Feeds.Queries.GetAllForFolder;

public record GetAllFeedsForFolderQuery(int FolderId, int RequesterId) : IRequest<IList<Feed>>;

internal class GetAllFeedsForFolderQueryValidator : AbstractValidator<GetAllFeedsForFolderQuery>
{
    public GetAllFeedsForFolderQueryValidator()
    {
        RuleFor(e => e.FolderId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(e => e.RequesterId)
            .NotEmpty()
            .GreaterThan(0);
    }
}