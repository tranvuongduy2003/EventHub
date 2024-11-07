using EventHub.Shared.SeedWork;

namespace EventHub.Abstractions.Services;

/// <summary>
/// Defines a contract for an email service that provides various email functionalities.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends an email with the specified content.
    /// </summary>
    /// <param name="mailContent">
    /// An instance of <see cref="MailContent"/> containing the details of the email to be sent, such as recipient, subject, and body.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    Task SendMail(MailContent mailContent);

    /// <summary>
    /// Sends an email asynchronously with the specified recipient, subject, and HTML message.
    /// </summary>
    /// <param name="email">
    /// The recipient's email address.
    /// </param>
    /// <param name="subject">
    /// The subject of the email.
    /// </param>
    /// <param name="htmlMessage">
    /// The HTML content of the email message.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    Task SendEmailAsync(string email, string subject, string htmlMessage);

    /// <summary>
    /// Sends a registration confirmation email asynchronously to the specified email address.
    /// </summary>
    /// <param name="email">
    /// The recipient's email address.
    /// </param>
    /// <param name="userName">
    /// The name of the user who is registering.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    Task SendRegistrationConfirmationEmailAsync(string email, string userName);

    /// <summary>
    /// Sends a password reset email asynchronously to the specified email address with a reset URL.
    /// </summary>
    /// <param name="email">
    /// The recipient's email address.
    /// </param>
    /// <param name="resetPasswordUrl">
    /// The URL that the recipient can use to reset their password.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    Task SendResetPasswordEmailAsync(string email, string resetPasswordUrl);
}