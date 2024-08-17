using FluentEmail.Core;
using RssReader.Application.Abstractions;

namespace RssReader.Infrastructure.Email;

internal class EmailService : IEmailService
{
    private readonly IFluentEmail _fluentEmail;
    private readonly static string _templatesPath = $"{Directory.GetCurrentDirectory()}/../RssReader/CommonUI/Views/Emails";

    public EmailService(IFluentEmail fluentEmail)
        => _fluentEmail = fluentEmail;

    public async Task SendEmailVerificationEmailAsync(string userEmail, string OTP)
    {
        await SendEmailAsync(
            userEmail, 
            "Verify your email", 
            $"{_templatesPath}/EmailVerificationOTP.cshtml", 
            OTP);
    }

    private async Task SendEmailAsync<T>(string email, string subject, string templateFilePath, T templateModel)
    {
        await _fluentEmail
            .To(email)
            .Subject(subject)
            .UsingTemplateFromFile(templateFilePath, templateModel)
            .SendAsync();
    }
}
