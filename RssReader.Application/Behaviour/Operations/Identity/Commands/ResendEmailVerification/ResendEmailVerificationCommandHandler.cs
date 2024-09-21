using FluentValidation;
using MediatR;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.DTOs.Notifications;
using RssReader.Application.Common.Exceptions.General;

namespace RssReader.Application.Behaviour.Operations.Identity.Commands.ResendEmailVerification;

internal class ResendEmailVerificationCommandHandler : BaseHandler, IRequestHandler<ResendEmailVerificationCommand>
{
    private readonly IPublisher _publisher;

    public ResendEmailVerificationCommandHandler(IPublisher publisher, IWorkUnit workUnit) : base(workUnit)
        => _publisher = publisher;

    public async Task Handle(ResendEmailVerificationCommand request, CancellationToken cancellationToken)
    {
        await new ResendEmailVerificationCommandValidator().ValidateAndThrowAsync(request, cancellationToken);

        // Validate user
        if (!await _workUnit.UsersRepository
                            .DoesInstanceExistAsync(request.UserId, cancellationToken))
            throw new EntityNotFoundException(nameof(User));

        await _publisher.Publish(new UserCreatedNotification(request.UserId), cancellationToken);
    }
}
