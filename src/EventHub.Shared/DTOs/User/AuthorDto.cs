using System.ComponentModel;
using System.Text.Json.Serialization;
using EventHub.Shared.Enums.User;

namespace EventHub.Shared.DTOs.User;

public class AuthorDto
{
    [Description("Unique identifier for the author.")]
    public string Id { get; set; }

    [Description("The username of the author.")]
    public string UserName { get; set; }

    [Description("The email address of the author.")]
    public string Email { get; set; }

    [Description("The phone number of the author.")]
    public string PhoneNumber { get; set; }

    [Description("The date of birth of the author.")]
    public DateTime? Dob { get; set; }

    [Description("The full name of the author.")]
    public string FullName { get; set; }

    [Description("The gender of the author.")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EGender? Gender { get; set; }

    [Description("The avatar URL or path for the author.")]
    public string? Avatar { get; set; }
}