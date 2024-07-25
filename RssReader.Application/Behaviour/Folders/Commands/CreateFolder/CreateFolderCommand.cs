using FluentValidation;
using MediatR;
using RssReader.Application.Common.DTOs;

namespace RssReader.Application.Behaviour.Folders.Commands.CreateFolder;

public record CreateFolderCommand(int RequesterId, string FolderName, int? ParentFolderId) : IRequest<Folder>;

internal class CreateFolderCommandValidator : AbstractValidator<CreateFolderCommand>
{
    public CreateFolderCommandValidator()
    {
        RuleFor(e => e.RequesterId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(e => e.FolderName)
            .NotEmpty()
            .MaximumLength(40);

        RuleFor(e => e.ParentFolderId)
            .GreaterThan(0);
    }
}