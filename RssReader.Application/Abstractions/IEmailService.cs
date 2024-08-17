namespace RssReader.Application.Abstractions;

public interface IEmailService
{
    Task SendEmailVerificationEmailAsync(string userEmail, string OTP);
}
