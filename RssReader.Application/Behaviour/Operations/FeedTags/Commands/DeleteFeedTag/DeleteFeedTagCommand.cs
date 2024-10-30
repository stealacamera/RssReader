using FluentValidation;
using MediatR;
using RssReader.Application.Common.Validation;

namespace RssReader.Application.Behaviour.Operations.FeedTags.Commands.DeleteFeedTag;

public record DeleteFeedTagCommand(int RequesterId, int TagId, int FeedSubscriptionId) : IRequest;

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

        RuleFor(e => e.FeedSubscriptionId)
            .NotEmpty()
            .GreaterThan(0);
    }
}