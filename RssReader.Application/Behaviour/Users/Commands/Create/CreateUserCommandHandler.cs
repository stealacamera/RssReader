using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions;
using RssReader.Domain.Abstractions;

namespace RssReader.Application.Behaviour.Users.Commands.Create;

internal class CreateUserCommandHandler : BaseCommandHandler, IRequestHandler<CreateUserCommand, User>
{
    public CreateUserCommandHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Validate request properties
        await new CreateUserCommandValidator().ValidateAndThrowAsync(request, cancellationToken);

        // Validate email uniqueness
        if (await _workUnit.UsersRepository
                           .GetByEmailAsync(request.Email, cancellationToken) != null)
            throw new ExistingEmailException();

        // Create user
        var newUser = new Domain.Entities.User
        {
            CreatedAt = DateTime.Now,
            Email = request.Email.Trim(),
            Username = request.Username?.Trim()
        };

        // Hash password
        PasswordHasher<Domain.Entities.User> passwordHasher = new();
        newUser.HashedPassword = passwordHasher.HashPassword(newUser, request.Password);

        // Register user
        await _workUnit.UsersRepository
                       .AddAsync(newUser, cancellationToken);

        await _workUnit.SaveChangesAsync();
        return new User(newUser.Id, newUser.Email, newUser.Username);
    }
}
