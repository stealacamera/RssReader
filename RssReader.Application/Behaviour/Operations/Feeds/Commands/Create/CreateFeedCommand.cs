using FluentValidation;
using MediatR;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Validation;

namespace RssReader.Application.Behaviour.Operations.Feeds.Commands.Create;

public record CreateFeedCommand(int RequesterId, string Url, string Name) : IRequest<Feed>;

internal class CreateFeedCommandValidator : Validator<CreateFeedCommand>
{
    public CreateFeedCommandValidator()
    {
        RuleFor(e => e.RequesterId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(e => e.Url)
            .NotEmpty()
            .MaximumLength(200)
            .ValidUrl();

        RuleFor(e => e.Name)
            .NotEmpty()
            .MaximumLength(80);
    }
}