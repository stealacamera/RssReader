using FluentValidation;
using MediatR;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Validation;

namespace RssReader.Application.Behaviour.Operations.Tags.Commands.Edit;

public record EditTagCommand : IRequest<Tag>
{
    public int TagId { get; }
    public int RequesterId { get; }
    public string NewTagName { get; }

    public EditTagCommand(int tagId, string newTagName, int requesterId)
    {
        RequesterId = requesterId;
        TagId = tagId;
        NewTagName = newTagName.Trim();
    }
}

internal class EditTagCommandValidator : Validator<EditTagCommand>
{
    public EditTagCommandValidator()
    {
        RuleFor(e => e.TagId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(e => e.NewTagName)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(e => e.RequesterId)
            .NotEmpty()
            .GreaterThan(0);
    }
}