using FluentEmail.Core;
using Microsoft.Extensions.Options;
using RssReader.Application.Abstractions;
using RssReader.Infrastructure.Options;

namespace RssReader.Infrastructure.Email;

// todo handle errors from mediatr notifications with just logs, so no errors in main stream
// fix email templates

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
        await SendEmailAsync(
            userEmail,
            "Verify your account email",
            $"Your password is {OTP}");
            //"RssReader.Infrastructure.Email.Templates.EmailVerificationOTP.cshtml",
            //OTP);
            //await RazorTemplateEngine.RenderAsync("/Templates/EmailVerificationOTP.cshtml", OTP));
    }

    private async Task SendEmailAsync(string email, string subject, string message)
    {
        await _fluentEmail.To(email)
            .Subject(subject)
            .Body(message)
            .SendAsync();
    }

    private void SendEmail(string email, string subject, string templatePath, object templateModel)
    {
        _fluentEmail.To(email)
            .Subject(subject)
            .UsingTemplateFromEmbedded(
                templatePath, 
                templateModel, 
                typeof(EmailService).Assembly)
            .Send();
        
        //var emailMessage = new MimeMessage();

        //emailMessage.From.Add(MailboxAddress.Parse(_emailOptions.Email));
        //emailMessage.To.Add(MailboxAddress.Parse(email));

        //emailMessage.Subject = subject;
        //emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage };

        //using (var emailClient = new SmtpClient())
        //{
        //    emailClient.Connect(_emailOptions.Host, _emailOptions.Port, MailKit.Security.SecureSocketOptions.StartTls);
        //    emailClient.Authenticate(_emailOptions.Email, _emailOptions.Password);
        //    emailClient.Send(emailMessage);
        //    emailClient.Disconnect(true);
        //}
    }
}
