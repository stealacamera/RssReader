using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.DTOs.Notifications;
using RssReader.Application.Common.Exceptions;

namespace RssReader.Application.Behaviour.Operations.Users.Commands.Create;

internal class CreateUserCommandHandler : BaseHandler, IRequestHandler<CreateUserCommand, User>
{
    private readonly IPublisher _publisher;

    public CreateUserCommandHandler(
        IWorkUnit workUnit,
        IPublisher publisher)
        : base(workUnit)
    {
        _publisher = publisher;
    }

    public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        await ValidateRequestAsync(request, cancellationToken);

        // Create user
        var newUser = new Domain.Entities.Identity.User
        {
            CreatedAt = DateTime.UtcNow,
            Email = request.Email,
            Username = request.Username
        };

        // Hash password
        PasswordHasher<Domain.Entities.Identity.User> passwordHasher = new();
        newUser.HashedPassword = passwordHasher.HashPassword(newUser, request.Password);

        // Register user
        await WrapInTransactionAsync(async () =>
        {
            // Save user locally
            await _workUnit.UsersRepository
                           .AddAsync(newUser, cancellationToken);

            await _workUnit.SaveChangesAsync();

            // Add role for user
            await _workUnit.UserRolesRepository
                           .AddAsync(new Domain.Entities.Identity.UserRole
                           {
                               CreatedAt = DateTime.UtcNow,
                               RoleId = (int)request.Role,
                               UserId = newUser.Id
                           }, cancellationToken);

            await _workUnit.SaveChangesAsync();

        });

        await _publisher.Publish(new UserCreatedNotification(newUser.Id));
        return new User(newUser.Id, newUser.Email, newUser.Username);
    }

    private async Task ValidateRequestAsync(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Validate request properties
        await new CreateUserCommandValidator().ValidateAndThrowAsync(request, cancellationToken);

        // Validate email uniqueness
        var existingUserEmail = await _workUnit.UsersRepository
                                               .GetByEmailAsync(request.Email, cancellationToken);

        if (existingUserEmail != null)
            throw new ExistingEmailException();
    }
}
