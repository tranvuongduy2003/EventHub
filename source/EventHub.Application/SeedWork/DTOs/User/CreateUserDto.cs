using System.Text.Json.Serialization;
using EventHub.Domain.Shared.Enums.User;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Application.SeedWork.DTOs.User;

public class CreateUserDto
{
    [SwaggerSchema("Email will be registered for the account")]
    public string Email { get; set; }

    [SwaggerSchema("PhoneNumber will be registered for the account")]
    public string PhoneNumber { get; set; }

    [SwaggerSchema("Date of birth of the user")]
    public DateTime? Dob { get; set; }

    [SwaggerSchema("Full name will be registered for the account")]
    public string FullName { get; set; }

    [SwaggerSchema("Password will be registered for the account")]
    public string Password { get; set; }

    [SwaggerSchema("User name will be registered for the account")]
    public string? UserName { get; set; } = string.Empty;

    [SwaggerSchema("Gender of the user")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EGender? Gender { get; set; }

    [SwaggerSchema("Biography of the user")]
    public string? Bio { get; set; }

    [SwaggerSchema("Avatar of the user")]
    public IFormFile? Avatar { get; set; }
}
