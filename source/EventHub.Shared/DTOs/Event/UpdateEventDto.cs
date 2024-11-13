using System.ComponentModel;
using System.Text.Json.Serialization;
using EventHub.Shared.Enums.Event;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.Event;

public class UpdateEventDto
{
    [SwaggerSchema("Unique identifier of the event")]
    [DefaultValue("b0b4c582-12b6-49fd-a9e6-8d901a9f9d37")]
    public Guid Id { get; set; }

    [SwaggerSchema("Cover image for the event")]
    public IFormFile CoverImage { get; set; }

    [DefaultValue("Tech Conference 2024")]
    [SwaggerSchema("Name of the event")]
    public string Name { get; set; } = string.Empty;

    [DefaultValue("A conference focusing on the latest trends in technology.")]
    [SwaggerSchema("Detailed description of the event")]
    public string Description { get; set; } = string.Empty;

    [DefaultValue("123 Tech Street, Silicon Valley, CA")]
    [SwaggerSchema("Location where the event will take place")]
    public string Location { get; set; } = string.Empty;

    [DefaultValue(typeof(DateTime), "2024-10-01T09:00:00Z")]
    [SwaggerSchema("Start time of the event (UTC)")]
    public DateTime StartTime { get; set; } = DateTime.UtcNow;

    [DefaultValue(typeof(DateTime), "2024-10-01T17:00:00Z")]
    [SwaggerSchema("End time of the event (UTC)")]
    public DateTime EndTime { get; set; } = DateTime.UtcNow;

    [DefaultValue(10.5)]
    [SwaggerSchema("Optional promotion percentage for the event")]
    public double? Promotion { get; set; } = 0;

    [SwaggerSchema("Type of event cycle (e.g., SINGLE, RECURRING)")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [DefaultValue(nameof(EEventCycleType.SINGLE))]
    public EEventCycleType EventCycleType { get; set; } = EEventCycleType.SINGLE;

    [SwaggerSchema("Payment type for the event (e.g., FREE, PAID)")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [DefaultValue(nameof(EEventPaymentType.PAID))]
    public EEventPaymentType EventPaymentType { get; set; } = EEventPaymentType.FREE;

    [DefaultValue(false)]
    [SwaggerSchema("Indicates if the event is private")]
    public bool IsPrivate { get; set; } = false;

    [SwaggerSchema("List of categories associated with the event")]
    [DefaultValue(new[] { "Technology", "Conference" })]
    public List<Guid> Categories { get; set; } = new();

    [SwaggerSchema("Types of tickets available for the event")]
    public IEnumerable<string>? TicketTypes { get; set; } = new List<string>();

    [SwaggerSchema("Optional reasons associated with the event")]
    [DefaultValue(new[] { "Networking", "Learning" })]
    public IEnumerable<string>? Reasons { get; set; } = new List<string>();

    [SwaggerSchema("Additional images for the event")]
    public IFormFileCollection? EventSubImages { get; set; } = null;

    [SwaggerSchema("Email content to be sent for event notifications")]
    [DefaultValue(typeof(UpdateEmailContentDto))]
    public UpdateEmailContentDto? EmailContent { get; set; } = null;
}