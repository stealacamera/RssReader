using FluentValidation;
using MediatR;
using RssReader.Application.Common.DTOs;

namespace RssReader.Application.Behaviour.Operations.Folders.Commands.Create;

public record CreateFolderCommand : IRequest<Folder>
{
    public int RequesterId { get; }
    public string FolderName { get; }
    public int? ParentFolderId { get; }

    public CreateFolderCommand(int requesterId, string folderName, int? parentFolderId)
    {
        RequesterId = requesterId;
        FolderName = folderName.Trim();
        ParentFolderId = parentFolderId;
    }
}

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