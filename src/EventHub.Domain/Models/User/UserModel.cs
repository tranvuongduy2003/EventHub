using System.Text.Json.Serialization;
using EventHub.Domain.Enums.User;

namespace EventHub.Domain.Models.User;

public class UserModel
{
    public string Id { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public DateTime? Dob { get; set; }

    public string FullName { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EGender? Gender { get; set; }

    public string? Bio { get; set; }

    public string? Avatar { get; set; }

    public EUserStatus Status { get; set; }

    public int? NumberOfFollowers { get; set; } = 0;

    public int? NumberOfFolloweds { get; set; } = 0;

    public int? NumberOfFavourites { get; set; } = 0;

    public int? NumberOfCreatedEvents { get; set; } = 0;

    public List<string> Roles { get; set; }

    public List<string> FollowingIds { get; set; } = new();

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}