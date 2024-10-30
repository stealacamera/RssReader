using FluentValidation;
using MediatR;
using RssReader.Application.Common.Validation;

namespace RssReader.Application.Behaviour.Operations.Folders.Commands.Delete;

public record DeleteFolderCommand(int RequesterId, int FolderId) : IRequest;

internal class DeleteFolderCommandValidator : Validator<DeleteFolderCommand>
{
    public DeleteFolderCommandValidator()
    {
        RuleFor(e => e.RequesterId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(e => e.FolderId)
            .NotEmpty()
            .GreaterThan(0);
    }
}