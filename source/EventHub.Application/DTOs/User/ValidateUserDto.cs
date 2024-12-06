using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Application.DTOs.User;

public class ValidateUserDto
{
    [SwaggerSchema("Email will be registered for the account")]
    public string Email { get; set; }

    [SwaggerSchema("PhoneNumber will be registered for the account")]
    public string PhoneNumber { get; set; }

    [SwaggerSchema("Full name will be registered for the account")]
    public string FullName { get; set; }
}
