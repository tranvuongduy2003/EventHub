using System.Text.Json.Serialization;
using EventHub.Domain.Shared.Enums.User;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Application.DTOs.User;

public class UpdateUserDto
{
    [SwaggerSchema("Id of the user")]
    public Guid Id { get; set; }

    [SwaggerSchema("Email will be registered for the account")]
    public string Email { get; set; }

    [SwaggerSchema("PhoneNumber will be registered for the account")]
    public string PhoneNumber { get; set; }

    [SwaggerSchema("Date of birth of the user")]
    public DateTime? Dob { get; set; }

    [SwaggerSchema("Full name will be registered for the account")]
    public string FullName { get; set; }

    [SwaggerSchema("User name will be registered for the account")]
    public string? UserName { get; set; }

    [SwaggerSchema("Gender of the user")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EGender? Gender { get; set; }

    [SwaggerSchema("Biography of the user")]
    public string? Bio { get; set; }

    [SwaggerSchema("Avatar of the user")]
    public IFormFile? Avatar { get; set; }
}
