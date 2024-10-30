using FluentValidation;
using MediatR;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions;
using RssReader.Application.Common.Exceptions.General;

namespace RssReader.Application.Behaviour.Operations.Identity.Commands.VerifyOTP;

internal class VerifyOTPCommandHandler : BaseHandler, IRequestHandler<VerifyOTPCommand, bool>
{
    public VerifyOTPCommandHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task<bool> Handle(VerifyOTPCommand request, CancellationToken cancellationToken)
    {
        var otp = await ValidateRequestAsync(request, cancellationToken);

        // Verify OTP
        if (otp.Password != request.Password)
        {
            otp.RetryAttempts++;
            await _workUnit.SaveChangesAsync();

            return false;
        }
        else
        {
            var user = await _workUnit.UsersRepository
                                      .GetByIdAsync(request.RequesterId);

            // Confirm user email & delete OTP
            user!.IsEmailConfirmed = true;
            _workUnit.OTPsRepository.Delete(otp);
            
            await _workUnit.SaveChangesAsync();
            return true;
        }
    }

    private async Task<Domain.Entities.Identity.OTP> ValidateRequestAsync(VerifyOTPCommand request, CancellationToken cancellationToken)
    {
        // Validate request properties
        await new ValidateOTPCommandValidator().ValidateAndThrowAsync(request, cancellationToken);

        // Validate user
        var requester = await _workUnit.UsersRepository
                                       .GetByIdAsync(request.RequesterId);

        if (requester == null)
            throw new EntityNotFoundException(nameof(User));
        else if (requester.IsEmailConfirmed)
            throw new UnauthorizedException();

        // Validate OTP
        var otp = await _workUnit.OTPsRepository
                                 .GetByUserIdAsync(request.RequesterId);

        if (otp == null || otp.RetryAttempts >= 3 || otp.ExpiryDate < DateTime.UtcNow)
            throw new InvalidOTPException();

        return otp;
    }
}
