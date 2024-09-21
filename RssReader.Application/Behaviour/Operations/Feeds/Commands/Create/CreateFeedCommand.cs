using FluentValidation;
using MediatR;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Validation;

namespace RssReader.Application.Behaviour.Operations.Feeds.Commands.Create;

public record CreateFeedCommand : IRequest<Feed>
{
    public int RequesterId { get; }
    public int FolderId { get; }
    public string Url { get; }
    public string? Name { get; }

    public CreateFeedCommand(int requesterId, int folderId, string url, string? name = null)
    {
        RequesterId = requesterId;
        FolderId = folderId;
        Url = url.Trim();
        Name = name?.Trim();
    }
}

internal class CreateFeedCommandValidator : Validator<CreateFeedCommand>
{
    public CreateFeedCommandValidator()
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