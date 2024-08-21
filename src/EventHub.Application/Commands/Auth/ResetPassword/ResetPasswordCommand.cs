using EventHub.Shared.DTOs.User;
using MediatR;

namespace EventHub.Application.Commands.Auth.ResetPassword;

// <summary>
/// Represents a command to reset a user's password.
/// </summary>
public class ResetPasswordCommand : IRequest<bool>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ResetPasswordCommand"/> class.
    /// </summary>
    /// <param name="dto">
    /// The data transfer object containing the necessary information to reset the user's password.
    /// </param>
    public ResetPasswordCommand(ResetUserPasswordDto dto)
        => (NewPassword, Email, Token) = (dto.NewPassword, dto.Email, dto.Token);

    /// <summary>
    /// Gets or sets the new password to be set for the user.
    /// </summary>
    /// <value>
    /// A string representing the new password.
    /// </value>
    public string NewPassword { get; set; }

    /// <summary>
    /// Gets or sets the email address associated with the user account whose password is being reset.
    /// </summary>
    /// <value>
    /// A string representing the email address of the user.
    /// </value>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the token used to verify the password reset request.
    /// </summary>
    /// <value>
    /// A string representing the password reset token.
    /// </value>
    public string Token { get; set; }
}