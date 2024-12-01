using System.Text.Json.Serialization;
using EventHub.Shared.DTOs.Category;
using EventHub.Shared.DTOs.User;
using EventHub.Shared.Enums.Event;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.Event;

public class EventDto
{
    [SwaggerSchema("Unique identifier of the event")]
    public Guid Id { get; set; }

    [SwaggerSchema("URL of the cover image for the event")]
    public string CoverImageUrl { get; set; } = string.Empty;

    [SwaggerSchema("Name of the event")]
    public string Name { get; set; } = string.Empty;

    [SwaggerSchema("Detailed description of the event")]
    public string Description { get; set; } = string.Empty;

    [SwaggerSchema("Location where the event will be held")]
    public string Location { get; set; } = string.Empty;

    [SwaggerSchema("Start time of the event (UTC)")]
    public DateTime StartTime { get; set; } = DateTime.UtcNow;

    [SwaggerSchema("End time of the event (UTC)")]
    public DateTime EndTime { get; set; } = DateTime.UtcNow;

    [SwaggerSchema("Optional promotion discount for the event (in percentage)")]
    public double? Promotion { get; set; } = 0;

    [SwaggerSchema("The number of times this event has been marked as a favorite")]
    public int? NumberOfFavourites { get; set; } = 0;

    [SwaggerSchema("The number of times this event has been shared")]
    public int? NumberOfShares { get; set; } = 0;

    [SwaggerSchema("The total number of tickets sold for this event")]
    public int? NumberOfSoldTickets { get; set; } = 0;

    [SwaggerSchema("Current status of the event")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EEventStatus? Status { get; set; }

    [SwaggerSchema("Cycle type of the event (e.g., ONETIME, RECURRING)")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EEventCycleType EventCycleType { get; set; } = EEventCycleType.ONETIME;

    [SwaggerSchema("Payment type for the event (e.g., FREE, PAID)")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EEventPaymentType EventPaymentType { get; set; } = EEventPaymentType.FREE;

    [SwaggerSchema("Indicates if the event is private")]
    public bool IsPrivate { get; set; } = false;

    [SwaggerSchema("The author who created the event")]
    public UserDto? Author { get; set; } = null;

    [SwaggerSchema("Categories associated with the event")]
    public List<CategoryDto> Categories { get; set; }

    [SwaggerSchema("The date and time when the event was created (UTC)")]
    public DateTime CreatedAt { get; set; }

    [SwaggerSchema("The date and time when the event was last updated (optional)")]
    public DateTime? UpdatedAt { get; set; }
}
