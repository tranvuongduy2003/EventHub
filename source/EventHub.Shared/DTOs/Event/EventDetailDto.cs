using System.ComponentModel;
using System.Text.Json.Serialization;
using EventHub.Shared.DTOs.Category;
using EventHub.Shared.DTOs.User;
using EventHub.Shared.Enums.Event;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.Event;

public class EventDetailDto
{
    [SwaggerSchema("Unique identifier for the event")]
    [DefaultValue("a1b2c3d4-e5f6-7890-ab12-cd34ef56gh78")]
    public Guid Id { get; set; }

    [SwaggerSchema("URL for the cover image of the event")]
    [DefaultValue("https://example.com/cover-image.jpg")]
    public string CoverImageUrl { get; set; } = string.Empty;

    [SwaggerSchema("Name of the event")]
    [DefaultValue("Tech Conference 2024")]
    public string Name { get; set; } = string.Empty;

    [SwaggerSchema("Detailed description of the event")]
    [DefaultValue("A conference focusing on the latest trends in technology.")]
    public string Description { get; set; } = string.Empty;

    [SwaggerSchema("Location where the event will take place")]
    [DefaultValue("123 Tech Street, Silicon Valley, CA")]
    public string Location { get; set; } = string.Empty;

    [SwaggerSchema("Start time of the event (UTC)")]
    [DefaultValue(typeof(DateTime), "2024-10-01T09:00:00Z")]
    public DateTime StartTime { get; set; } = DateTime.UtcNow;

    [SwaggerSchema("End time of the event (UTC)")]
    [DefaultValue(typeof(DateTime), "2024-10-01T17:00:00Z")]
    public DateTime EndTime { get; set; } = DateTime.UtcNow;

    [SwaggerSchema("Promotion percentage for the event")]
    [DefaultValue(10.5)]
    public double? Promotion { get; set; } = 0;

    [SwaggerSchema("Number of users who have marked the event as favourite")]
    [DefaultValue(0)]
    public int? NumberOfFavourites { get; set; } = 0;

    [SwaggerSchema("Number of times the event has been shared")]
    [DefaultValue(0)]
    public int? NumberOfShares { get; set; } = 0;

    [SwaggerSchema("Number of tickets sold for the event")]
    [DefaultValue(0)]
    public int? NumberOfSoldTickets { get; set; } = 0;

    [SwaggerSchema("Current status of the event")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [DefaultValue(nameof(EEventStatus.OPENING))]
    public EEventStatus? Status { get; set; }

    [SwaggerSchema("Type of event cycle (e.g., SINGLE, RECURRING)")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [DefaultValue(nameof(EEventCycleType.SINGLE))]
    public EEventCycleType EventCycleType { get; set; } = EEventCycleType.SINGLE;

    [SwaggerSchema("Payment type for the event (e.g., FREE, PAID)")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [DefaultValue(nameof(EEventPaymentType.FREE))]
    public EEventPaymentType EventPaymentType { get; set; } = EEventPaymentType.FREE;

    [SwaggerSchema("Indicates if the event is private")]
    [DefaultValue(false)]
    public bool IsPrivate { get; set; } = false;

    [SwaggerSchema("List of URLs for additional event images")]
    [DefaultValue(new[] { "https://example.com/sub-image1.jpg", "https://example.com/sub-image2.jpg" })]
    public List<string> EventSubImageUrls { get; set; } = new();

    [SwaggerSchema("List of reasons associated with the event")]
    public List<ReasonDto> Reasons { get; set; } = new();

    [SwaggerSchema("Categories associated with the event")]
    public List<CategoryDto> Categories { get; set; } = new();

    [SwaggerSchema("List of ticket types available for the event")]
    public List<TicketTypeDto> TicketTypes { get; set; } = new();

    [SwaggerSchema("Email content related to the event")]
    public EmailContentDto EmailContent { get; set; }

    [SwaggerSchema("Author of the event")] public AuthorDto Author { get; set; }

    [SwaggerSchema("Creation timestamp of the event")]
    [DefaultValue(typeof(DateTime), "2024-09-01T12:00:00Z")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [SwaggerSchema("Timestamp of the last update to the event")]
    public DateTime? UpdatedAt { get; set; } = null;
}