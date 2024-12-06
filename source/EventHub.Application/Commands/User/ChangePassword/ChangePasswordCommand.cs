using EventHub.Application.DTOs.User;
using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.User.ChangePassword;

/// <summary>
/// Represents a command to change the password of a user.
/// </summary>
/// <remarks>
/// This command is used to update the password for a specific user, requiring both the old and new passwords.
/// </remarks>
public class ChangePasswordCommand : ICommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangePasswordCommand"/> class.
    /// </summary>
    /// <param name="userId">
    /// The unique identifier of the user whose password is to be changed.
    /// </param>
    /// <param name="request">
    /// The data transfer object containing the old and new password details.
    /// </param>
    public ChangePasswordCommand(Guid userId, UpdateUserPasswordDto request)
        => (UserId, OldPassword, NewPassword) = (userId, request.OldPassword, request.NewPassword);

    /// <summary>
    /// Gets or sets the unique identifier of the user whose password is to be changed.
    /// </summary>
    /// <value>
    /// A <see cref="Guid"/> representing the unique identifier of the user.
    /// </value>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the old password of the user.
    /// </summary>
    /// <value>
    /// A string representing the user's old password.
    /// </value>
    public string OldPassword { get; set; }

    /// <summary>
    /// Gets or sets the new password for the user.
    /// </summary>
    /// <value>
    /// A string representing the new password that the user wants to set.
    /// </value>
    public string NewPassword { get; set; }
}
