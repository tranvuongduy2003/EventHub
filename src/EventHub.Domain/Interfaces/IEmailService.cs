using EventHub.Domain.Common.Models;

namespace EventHub.Domain.Interfaces;

public interface IEmailService
{
    Task SendMail(MailContent mailContent);

    Task SendEmailAsync(string email, string subject, string htmlMessage);

    Task SendRegistrationConfirmationEmailAsync(string email, string userName);

    Task SendResetPasswordEmailAsync(string email, string resetPasswordUrl);
}