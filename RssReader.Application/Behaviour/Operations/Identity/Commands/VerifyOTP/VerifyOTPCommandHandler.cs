using MediatR;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions;

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

    private async Task<Domain.Entities.OTP> ValidateRequestAsync(VerifyOTPCommand request, CancellationToken cancellationToken)
    {
        // Validate request properties
        var validationDetails = await new ValidateOTPCommandValidator().ValidateAsync(request, cancellationToken);

        if (!validationDetails.IsValid)
            throw new ValidationException(validationDetails.ToDictionary());

        // Validate user
        if (!await _workUnit.UsersRepository
                            .DoesInstanceExistAsync(request.RequesterId))
            throw new EntityNotFoundException(nameof(User));

        // Validate OTP
        var otp = await _workUnit.OTPsRepository
                                 .GetByUserIdAsync(request.RequesterId);

        if (otp == null)
            throw new EntityNotFoundException("OTP");
        else if (otp.RetryAttempts >= 3 || otp.ExpiryDate < DateTime.UtcNow)
            throw new InvalidOTPException();

        return otp;
    }
}
