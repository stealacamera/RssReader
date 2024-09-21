using FluentValidation;
using MediatR;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Validation;

namespace RssReader.Application.Behaviour.Operations.FeedTags.Commands.AddTagToFeed;

public record AddTagToFeedCommand(
    int RequesterId,
    int TagId,
    int FeedId)
    : IRequest<FeedTag>;

internal class AddTagToFeedCommandValidator : Validator<AddTagToFeedCommand>
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