using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.DTOs.Auth;

namespace EventHub.Application.Commands.Auth.ForgotPassword;

/// <summary>
/// Represents a command to initiate a password reset process for a user.
/// </summary>
public class ForgotPasswordCommand : ICommand<bool>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ForgotPasswordCommand"/> class.
    /// </summary>
    /// <param name="dto">
    /// The data transfer object containing the email address for the password reset request.
    /// </param>
    public ForgotPasswordCommand(ForgotPasswordDto dto)
        => Email = dto.Email;

    /// <summary>
    /// Gets or sets the email address associated with the user account for which the password reset is requested.
    /// </summary>
    /// <value>
    /// A string representing the email address of the user requesting the password reset.
    /// </value>
    public string Email { get; init; }
}