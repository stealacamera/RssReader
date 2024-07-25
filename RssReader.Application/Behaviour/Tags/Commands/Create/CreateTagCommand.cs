using FluentValidation;
using MediatR;
using RssReader.Application.Common.DTOs;

namespace RssReader.Application.Behaviour.Tags.Commands.Create;

public record CreateTagCommand(int RequesterId, string TagName) : IRequest<Tag>;

internal class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
{
    public CreateTagCommandValidator()
    {
        RuleFor(e => e.RequesterId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(e => e.TagName)
            .NotEmpty()
            .MaximumLength(30);
    }
}