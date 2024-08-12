using EventHub.Shared.SeedWork;

namespace EventHub.Domain.Services;

public interface IEmailService
{
    Task SendMail(MailContent mailContent);

    Task SendEmailAsync(string email, string subject, string htmlMessage);

    Task SendRegistrationConfirmationEmailAsync(string email, string userName);

    Task SendResetPasswordEmailAsync(string email, string resetPasswordUrl);
}