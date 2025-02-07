using System.Text.Json.Serialization;
using EventHub.Domain.Shared.Enums.User;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Application.SeedWork.DTOs.User;

public class UserDto
{
    [SwaggerSchema("Id of the user")]
    public Guid Id { get; set; }

    [SwaggerSchema("User name of the user")]
    public string UserName { get; set; }

    [SwaggerSchema("Email of the user")]
    public string Email { get; set; }

    [SwaggerSchema("Phone number of the user")]
    public string PhoneNumber { get; set; }

    [SwaggerSchema("Date of birth of the user")]
    public DateTime? Dob { get; set; }

    [SwaggerSchema("Full name of the user")]
    public string FullName { get; set; }

    [SwaggerSchema("Gender of the user")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EGender? Gender { get; set; }

    [SwaggerSchema("Biography of the user")]
    public string? Bio { get; set; }

    [SwaggerSchema("Avatar URL of the user")]
    public string? Avatar { get; set; }

    [SwaggerSchema("Status of the user's account")]
    public EUserStatus Status { get; set; }

    [SwaggerSchema("The number of people followed the user")]
    public int? NumberOfFollowers { get; set; } = 0;

    [SwaggerSchema("The number of people the user followed")]
    public int? NumberOfFolloweds { get; set; } = 0;

    [SwaggerSchema("The number of favourite events of the user")]
    public int? NumberOfFavourites { get; set; } = 0;

    [SwaggerSchema("The number of events the user created")]
    public int? NumberOfCreatedEvents { get; set; } = 0;

    [SwaggerSchema("Roles of the user")]
    public IEnumerable<string> Roles { get; set; } = null!;

    public bool IsInvited { get; set; }

    [SwaggerSchema("The datetime that the user was created")]
    public DateTime CreatedAt { get; set; }

    [SwaggerSchema("The last datetime that the user was updated")]
    public DateTime? UpdatedAt { get; set; }
}
