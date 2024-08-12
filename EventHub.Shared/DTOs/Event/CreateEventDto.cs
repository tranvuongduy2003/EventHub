using System.Text.Json.Serialization;
using EventHub.Shared.Enums.Event;
using Microsoft.AspNetCore.Http;

namespace EventHub.Shared.DTOs.Event;

public class CreateEventDto
{
    public string AuthorId { get; set; }

    public IFormFile CoverImage { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Location { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public List<string> CategoryIds { get; set; } = new();

    public double? Promotion { get; set; } = 0;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EEventCycleType EventCycleType { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EEventPaymentType EventPaymentType { get; set; }

    public bool IsPrivate { get; set; }

    public IEnumerable<string>? TicketTypes { get; set; } = new List<string>();

    public IEnumerable<string>? Reasons { get; set; } = new List<string>();

    public IFormFileCollection? EventSubImages { get; set; } = null;

    public CreateEmailContentDto? EmailContent { get; set; } = null;
}