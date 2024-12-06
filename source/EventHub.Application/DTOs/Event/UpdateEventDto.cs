using System.Text.Json.Serialization;
using EventHub.Domain.Shared.Enums.Event;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Application.DTOs.Event;

public class UpdateEventDto
{
    [SwaggerSchema("Unique identifier of the event")]
    public Guid Id { get; set; }

    [SwaggerSchema("Cover image for the event")]
    public IFormFile CoverImage { get; set; }

    [SwaggerSchema("Name of the event")]
    public string Name { get; set; } = string.Empty;

    [SwaggerSchema("Detailed description of the event")]
    public string Description { get; set; } = string.Empty;

    [SwaggerSchema("Location where the event will take place")]
    public string Location { get; set; } = string.Empty;

    [SwaggerSchema("Start time of the event (UTC)")]
    public DateTime StartTime { get; set; } = DateTime.UtcNow;

    [SwaggerSchema("End time of the event (UTC)")]
    public DateTime EndTime { get; set; } = DateTime.UtcNow;

    [SwaggerSchema("Optional promotion percentage for the event")]
    public double? Promotion { get; set; } = 0;

    [SwaggerSchema("Type of event cycle (e.g., ONETIME, RECURRING)")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EEventCycleType EventCycleType { get; set; } = EEventCycleType.ONETIME;

    [SwaggerSchema("Payment type for the event (e.g., FREE, PAID)")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EEventPaymentType EventPaymentType { get; set; } = EEventPaymentType.FREE;

    [SwaggerSchema("Indicates if the event is private")]
    public bool IsPrivate { get; set; } = false;

    [SwaggerSchema("List of categories associated with the event")]
    public List<Guid> Categories { get; set; } = new();

    [SwaggerSchema("Types of tickets available for the event")]
    public IEnumerable<string>? TicketTypes { get; set; } = new List<string>();

    [SwaggerSchema("Optional reasons associated with the event")]
    public IEnumerable<string>? Reasons { get; set; } = new List<string>();

    [SwaggerSchema("Additional images for the event")]
    public IFormFileCollection? EventSubImages { get; set; } = null;

    [SwaggerSchema("Email content to be sent for event notifications")]
    public UpdateEmailContentDto? EmailContent { get; set; } = null;
}
