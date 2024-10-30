using FluentEmail.Core;
using Microsoft.Extensions.Options;
using RssReader.Application.Abstractions;
using RssReader.Infrastructure.Options;

namespace RssReader.Infrastructure.Misc;

internal class EmailService : IEmailService
{
    private readonly IFluentEmail _fluentEmail;
    private readonly EmailOptions _emailOptions;

    public EmailService(IServiceProvider serviceProvider, IOptions<EmailOptions> emailOptions, IFluentEmail fluentEmail)
    {
        _fluentEmail = fluentEmail;
        _emailOptions = emailOptions.Value;
    }

    public async Task SendEmailVerificationEmailAsync(string userEmail, string OTP)
    {
        var message =
            "<p>Use the below code to activate your account & complete your registration. Careful, this code expires in 5 minutes.<br/>" +
            $"Your code: <b>{OTP}</b><br/><br/>" +
            "<i>If you didn't request this email, you can safely ignore it</i></p>";

        await SendEmailAsync(userEmail, "Verify your account email", message);
    }

    private async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        await _fluentEmail.To(email)
            .Subject(subject)
            .Body(htmlMessage, isHtml: true)
            .SendAsync();
    }
}
