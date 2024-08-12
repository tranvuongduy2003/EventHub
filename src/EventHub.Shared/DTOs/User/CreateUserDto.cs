using System.Text.Json.Serialization;
using EventHub.Shared.Enums.User;
using Microsoft.AspNetCore.Http;

namespace EventHub.Shared.DTOs.User;

public class CreateUserDto
{
    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public DateTime? Dob { get; set; }

    public string FullName { get; set; }

    public string Password { get; set; }

    public string? UserName { get; set; } = string.Empty;


    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EGender? Gender { get; set; }

    public string? Bio { get; set; }

    public IFormFile? Avatar { get; set; }
}