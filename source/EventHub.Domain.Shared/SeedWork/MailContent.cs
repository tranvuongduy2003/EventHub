namespace EventHub.Domain.Shared.SeedWork;

/// <summary>
/// Represents the content of an email message, including recipient, subject, and body.
/// </summary>
public class MailContent
{
    /// <summary>
    /// Gets or sets the recipient's email address.
    /// </summary>
    /// <value>
    /// A string representing the email address of the recipient.
    /// Defaults to an empty string.
    /// </value>
    public string To { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the subject of the email.
    /// </summary>
    /// <value>
    /// A string representing the subject line of the email.
    /// Defaults to an empty string.
    /// </value>
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the body of the email.
    /// </summary>
    /// <value>
    /// A string representing the content of the email body.
    /// Defaults to an empty string.
    /// </value>
    public string Body { get; set; } = string.Empty;
}
