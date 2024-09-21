using FluentValidation;
using MediatR;
using RssReader.Application.Common.Validation;

namespace RssReader.Application.Behaviour.Operations.FeedTags.Commands.DeleteFeedTag;

public record DeleteFeedTagCommand(int RequesterId, int TagId, int FeedId) : IRequest;

internal class DeleteFeedTagCommandValidator : Validator<DeleteFeedTagCommand>
{
    public DeleteFeedTagCommandValidator()
    {
        RuleFor(e => e.RequesterId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(e => e.TagId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(e => e.FeedId)
            .NotEmpty()
            .GreaterThan(0);
    }
}