using System.Text.Json.Serialization;
using EventHub.Domain.Enums.User;
using Microsoft.AspNetCore.Http;

namespace EventHub.Domain.DTOs.User;

public class UpdateUserDto
{
    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public DateTime? Dob { get; set; }

    public string FullName { get; set; }

    public string? UserName { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EGender? Gender { get; set; }

    public string? Bio { get; set; }

    public IFormFile? Avatar { get; set; }
}