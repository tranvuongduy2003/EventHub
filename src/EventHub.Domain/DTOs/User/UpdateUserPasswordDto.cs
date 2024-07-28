namespace EventHub.Domain.DTOs.User;

public class UpdateUserPasswordDto
{
    public string UserId { get; set; }

    public string OldPassword { get; set; }

    public string NewPassword { get; set; }
}