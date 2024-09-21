using FluentValidation;
using MediatR;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Validation;

namespace RssReader.Application.Behaviour.Operations.Feeds.Queries.GetAllForFolder;

public record GetAllFeedsForFolderQuery(int FolderId, int RequesterId) : IRequest<IList<Feed>>;

internal class GetAllFeedsForFolderQueryValidator : Validator<GetAllFeedsForFolderQuery>
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