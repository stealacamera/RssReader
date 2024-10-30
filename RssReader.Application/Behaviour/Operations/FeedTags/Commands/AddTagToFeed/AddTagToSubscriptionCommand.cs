using FluentValidation;
using MediatR;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Validation;

namespace RssReader.Application.Behaviour.Operations.FeedTags.Commands.AddTagToFeed;

public record AddTagToSubscriptionCommand(
    int RequesterId,
    int TagId,
    int FeedSubscriptionId)
    : IRequest<FeedSubscriptionTag>;

internal class AddTagToSubscriptionCommandValidator : Validator<AddTagToSubscriptionCommand>
{
    public AddTagToSubscriptionCommandValidator()
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