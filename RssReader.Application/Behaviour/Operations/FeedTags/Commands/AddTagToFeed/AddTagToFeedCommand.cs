using FluentValidation;
using MediatR;

namespace RssReader.Application.Behaviour.Operations.FeedTags.Commands.AddTagToFeed;

public record AddTagToFeedCommand(
    int RequesterId,
    int TagId,
    int FeedId)
    : IRequest;

internal class AddTagToFeedCommandValidator : AbstractValidator<AddTagToFeedCommand>
{
    public AddTagToFeedCommandValidator()
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