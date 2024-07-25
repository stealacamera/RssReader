using FluentValidation;
using MediatR;

namespace RssReader.Application.Behaviour.FeedTags.Commands.DeleteFeedTag;

public record DeleteFeedTagCommand(int RequesterId, int TagId, int FeedId) : IRequest;

internal class DeleteFeedTagCommandValidator : AbstractValidator<DeleteFeedTagCommand>
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