using System.ComponentModel;

namespace EventHub.Domain.DTOs.Auth;

public class SignInDto
{
    //PhoneNumber or Email
    [DefaultValue("admin@gmail.com")] public string Identity { get; set; }

    [DefaultValue("Admin@123")] public string Password { get; set; }
}