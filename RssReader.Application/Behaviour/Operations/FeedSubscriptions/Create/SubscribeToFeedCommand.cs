using FluentValidation;
using MediatR;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Validation;

namespace RssReader.Application.Behaviour.Operations.FeedSubscriptions.Create;

public record SubscribeToFeedCommand : IRequest<FeedSubscription>
{
    public int RequesterId { get; }
    public int FolderId { get; }
    public string Url { get; }
    public string? Name { get; }

    public SubscribeToFeedCommand(int requesterId, int folderId, string url, string? name = null)
    {
        RequesterId = requesterId;
        FolderId = folderId;
        Url = url.Trim();
        Name = name?.Trim();
    }
}

internal class SubscribeToFeedCommandValidator : Validator<SubscribeToFeedCommand>
{
    public SubscribeToFeedCommandValidator()
    {
        RuleFor(e => e.FolderId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(e => e.RequesterId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(e => e.Url)
            .NotEmpty()
            .MaximumLength(200)
            .ValidUrl();

        When(
            e => e.Name != null,
            () => RuleFor(e => e.Name)
                    .MaximumLength(80));
    }
}