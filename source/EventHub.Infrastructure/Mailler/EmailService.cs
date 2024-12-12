using System.Globalization;
using EventHub.Application.SeedWork.Abstractions;
using EventHub.Domain.Shared.SeedWork;
using EventHub.Domain.Shared.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace EventHub.Infrastructure.Mailler;

/// <summary>
/// Provides functionality for sending emails using the specified email settings and logging information.
/// </summary>
public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    private readonly ILogger<EmailService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailService"/> class.
    /// </summary>
    /// <param name="emailSettings">
    /// An instance of <see cref="EmailSettings"/> containing configuration settings required for sending emails.
    /// </param>
    /// <param name="logger">
    /// An instance of <see cref="ILogger{EmailService}"/> used to log information and errors for the <see cref="EmailService"/>.
    /// </param>
    public EmailService(EmailSettings emailSettings, ILogger<EmailService> logger)
    {
        _emailSettings = emailSettings;
        _logger = logger;
        _logger.LogInformation("Create EmailService");
    }

    public async Task SendMail(MailContent mailContent)
    {
        using var email = new MimeMessage();
        email.Sender = new MailboxAddress(_emailSettings.DisplayName, _emailSettings.Email);
        email.From.Add(new MailboxAddress(_emailSettings.DisplayName, _emailSettings.Email));
        email.To.Add(MailboxAddress.Parse(mailContent.To));
        email.Subject = mailContent.Subject;


        var builder = new BodyBuilder
        {
            HtmlBody = mailContent.Body
        };
        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();

        try
        {
            await smtp.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_emailSettings.Email, _emailSettings.Password);
            await smtp.SendAsync(email);
        }
        catch (Exception ex)
        {
            Directory.CreateDirectory("mailssave");
            string emailsavefile = string.Format(CultureInfo.InvariantCulture, @"mailssave/{0}.eml", Guid.NewGuid());
            await email.WriteToAsync(emailsavefile);

            _logger.LogError(ex, "Email sending email, save at - {FileName}", emailsavefile);
        }

        await smtp.DisconnectAsync(true);

        _logger.LogInformation("Send mail to {ToAddress}", mailContent.To);
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
        string FullPath = Path.Combine(Environment.CurrentDirectory, "Templates/", "SignUpEmailTemplate.html");

        var str = new StreamReader(FullPath);

        string mailText = await str.ReadToEndAsync();

        str.Close();

        mailText = mailText.Replace("[userName]", userName);

        await SendEmailAsync(email, "Registration Confirmation", mailText);
    }

    public async Task SendResetPasswordEmailAsync(string email, Uri resetPasswordUrl)
    {
        string FullPath = Path.Combine("Templates/", "ResetPasswordEmailTemplate.html");

        var str = new StreamReader(FullPath);

        string mailText = await str.ReadToEndAsync();

        str.Close();

        mailText = mailText.Replace("[resetPasswordUrl]", resetPasswordUrl.ToString());

        await SendEmailAsync(email, "Reset Your Password", mailText);
    }
}
