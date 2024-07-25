using FluentValidation;
using MediatR;
using RssReader.Application.Common.DTOs;

namespace RssReader.Application.Behaviour.Tags.Commands.Edit;

public record EditTagCommand(
    int TagId, 
    string NewTagName, 
    int RequesterId) 
    : IRequest<Tag>;

internal class EditTagCommandValidator : AbstractValidator<EditTagCommand>
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