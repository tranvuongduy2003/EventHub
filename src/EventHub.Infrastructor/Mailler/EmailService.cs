using EventHub.Domain.Common.Models;
using EventHub.Domain.Interfaces;
using EventHub.Domain.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace EventHub.Infrastructor.Mailler;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    private readonly ILogger<EmailService> _logger;

    public EmailService(EmailSettings emailSettings, ILogger<EmailService> logger)
    {
        _emailSettings = emailSettings;
        _logger = logger;
        _logger.LogInformation("Create EmailService");
    }

    public async Task SendMail(MailContent mailContent)
    {
        var email = new MimeMessage();
        email.Sender = new MailboxAddress(_emailSettings.DisplayName, _emailSettings.Email);
        email.From.Add(new MailboxAddress(_emailSettings.DisplayName, _emailSettings.Email));
        email.To.Add(MailboxAddress.Parse(mailContent.To));
        email.Subject = mailContent.Subject;


        var builder = new BodyBuilder();
        builder.HtmlBody = mailContent.Body;
        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();

        try
        {
            smtp.Connect(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailSettings.Email, _emailSettings.Password);
            await smtp.SendAsync(email);
        }
        catch (Exception ex)
        {
            Directory.CreateDirectory("mailssave");
            var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
            await email.WriteToAsync(emailsavefile);

            _logger.LogInformation("Email sending email, save at - " + emailsavefile);
            _logger.LogError(ex.Message);
        }

        smtp.Disconnect(true);

        _logger.LogInformation("send mail to " + mailContent.To);
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        await SendMail(new MailContent
        {
            To = email,
            Subject = subject,
            Body = htmlMessage
        });
    }

    public async Task SendRegistrationConfirmationEmailAsync(string email, string userName)
    {
        var FullPath = Path.Combine("Templates/", "SignUpEmailTemplate.html");

        var str = new StreamReader(FullPath);

        var mailText = str.ReadToEnd();

        str.Close();

        mailText = mailText.Replace("[userName]", userName);

        await SendEmailAsync(email, "Registration Confirmation", mailText);
    }

    public async Task SendResetPasswordEmailAsync(string email, string resetPasswordUrl)
    {
        var FullPath = Path.Combine("Templates/", "ResetPasswordEmailTemplate.html");

        var str = new StreamReader(FullPath);

        var mailText = str.ReadToEnd();

        str.Close();

        mailText = mailText.Replace("[resetPasswordUrl]", resetPasswordUrl);

        await SendEmailAsync(email, "Reset Your Password", mailText);
    }
}