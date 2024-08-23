using System.ComponentModel;
using System.Text.Json.Serialization;
using EventHub.Shared.Enums.User;

namespace EventHub.Shared.DTOs.User;

public class UserDto
{
    [Description("Id of the user")]
    public Guid Id { get; set; }

    [Description("User name of the user")]
    public string UserName { get; set; }

    [Description("Email of the user")]
    public string Email { get; set; }

    [Description("Phone number of the user")]
    public string PhoneNumber { get; set; }

    [Description("Date of birth of the user")]
    public DateTime? Dob { get; set; }

    [Description("Full name of the user")]
    public string FullName { get; set; }

    [Description("Gender of the user")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EGender? Gender { get; set; }

    [Description("Biography of the user")]
    public string? Bio { get; set; }

    [Description("Avatar URL of the user")]
    public string? Avatar { get; set; }

    [Description("Status of the user's account")]
    public EUserStatus Status { get; set; }

    [Description("The number of people followed the user")]
    public int? NumberOfFollowers { get; set; } = 0;

    [Description("The number of people the user followed")]
    public int? NumberOfFolloweds { get; set; } = 0;

    [Description("The number of favourite events of the user")]
    public int? NumberOfFavourites { get; set; } = 0;

    [Description("The number of events the user created")]
    public int? NumberOfCreatedEvents { get; set; } = 0;

    [Description("Roles of the user")]
    public IEnumerable<string> Roles { get; set; } = null;

    [Description("The datetime that the user was created")]
    public DateTime CreatedAt { get; set; }

    [Description("The last datetime that the user was updated")]
    public DateTime? UpdatedAt { get; set; }
}