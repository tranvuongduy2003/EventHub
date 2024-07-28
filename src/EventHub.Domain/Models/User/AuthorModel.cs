using System.Text.Json.Serialization;
using EventHub.Domain.Enums.User;

namespace EventHub.Domain.Models.User;

public class AuthorModel
{
    public string Id { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public DateTime? Dob { get; set; }

    public string FullName { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EGender? Gender { get; set; }

    public string? Avatar { get; set; }
}