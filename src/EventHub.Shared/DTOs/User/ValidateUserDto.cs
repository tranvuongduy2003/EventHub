using System.ComponentModel;

namespace EventHub.Shared.DTOs.User;

public class ValidateUserDto
{
    [DefaultValue("user123@gmail.com")]
    [Description("Email will be registered for the account")]
    public string Email { get; set; }

    [DefaultValue("+84123456789")]
    [Description("PhoneNumber will be registered for the account")]
    public string PhoneNumber { get; set; }

    [DefaultValue("User 123")]
    [Description("Full name will be registered for the account")]
    public string FullName { get; set; }
}