using System.ComponentModel;
using System.Text.Json.Serialization;
using EventHub.Shared.Enums.User;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.User;

public class AuthorDto
{
    [SwaggerSchema("Unique identifier for the author.")]
    public string Id { get; set; }

    [SwaggerSchema("The username of the author.")]
    public string UserName { get; set; }

    [SwaggerSchema("The email address of the author.")]
    public string Email { get; set; }

    [SwaggerSchema("The phone number of the author.")]
    public string PhoneNumber { get; set; }

    [SwaggerSchema("The date of birth of the author.")]
    public DateTime? Dob { get; set; }

    [SwaggerSchema("The full name of the author.")]
    public string FullName { get; set; }

    [SwaggerSchema("The gender of the author.")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EGender? Gender { get; set; }

    [SwaggerSchema("The avatar URL or path for the author.")]
    public string? Avatar { get; set; }
}