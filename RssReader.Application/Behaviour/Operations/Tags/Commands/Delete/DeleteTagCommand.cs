using FluentValidation;
using MediatR;
using RssReader.Application.Common.Validation;

namespace RssReader.Application.Behaviour.Operations.Tags.Commands.Delete;

public record DeleteTagCommand(int RequesterId, int TagId) : IRequest;

internal class DeleteTagCommandValidator : Validator<DeleteTagCommand>
{
    public DeleteTagCommandValidator()
    {
        RuleFor(e => e.RequesterId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(e => e.TagId)
            .NotEmpty()
            .GreaterThan(0);
    }
}