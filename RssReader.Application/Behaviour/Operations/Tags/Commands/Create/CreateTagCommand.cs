using FluentValidation;
using MediatR;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Validation;

namespace RssReader.Application.Behaviour.Operations.Tags.Commands.Create;

public record CreateTagCommand : IRequest<Tag>
{
    public int RequesterId { get; }
    public string TagName { get; }

    public CreateTagCommand(int requesterId, string tagName)
    {
        RequesterId = requesterId;
        TagName = tagName.Trim();
    }
}

internal class CreateTagCommandValidator : Validator<CreateTagCommand>
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