using FluentValidation;
using MediatR;
using RssReader.Application.Common.DTOs;

namespace RssReader.Application.Behaviour.Feeds.Commands.Create;

public record CreateFeedCommand(int RequesterId, int FolderId, string Url, string? Name = null) : IRequest<Feed>;

internal class CreateFeedCommandValidator : AbstractValidator<CreateFeedCommand>
{
    public CreateFeedCommandValidator()
    {
        RuleFor(e => e.FolderId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(e => e.RequesterId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(e => e.Name)
            .MaximumLength(80);

        RuleFor(e => e.Url)
            .NotEmpty()
            .MaximumLength(200)
            .Must(url =>
            {
                // Validate url format
                Uri? uri = null;
                
                if (!Uri.TryCreate(url, UriKind.Absolute, out uri) || uri == null)
                    return false;

                return true;
            });
    }
}