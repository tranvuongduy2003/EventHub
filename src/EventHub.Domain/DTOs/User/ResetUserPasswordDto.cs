namespace EventHub.Domain.DTOs.User;

public class ResetUserPasswordDto
{
    public string NewPassword { get; set; }

    public string Email { get; set; }

    public string Token { get; set; }
}