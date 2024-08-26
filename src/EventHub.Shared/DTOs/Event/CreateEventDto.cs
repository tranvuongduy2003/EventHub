using System.Text.Json.Serialization;
using EventHub.Shared.Enums.Event;
using Microsoft.AspNetCore.Http;

namespace EventHub.Shared.DTOs.Event;

public class CreateEventDto
{
    public IFormFile CoverImage { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public string Location { get; set; } = string.Empty;

    public DateTime StartTime { get; set; } = DateTime.UtcNow;

    public DateTime EndTime { get; set; } = DateTime.UtcNow;

    public double? Promotion { get; set; } = 0;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EEventStatus? Status { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EEventCycleType EventCycleType { get; set; } = EEventCycleType.SINGLE;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EEventPaymentType EventPaymentType { get; set; } = EEventPaymentType.FREE;

    public bool IsPrivate { get; set; } = false;
    
    public Guid AuthorId { get; set; }

    public List<string> Categories { get; set; } = new();
    
    public IEnumerable<CreateTicketTypeDto>? TicketTypes { get; set; } = new List<CreateTicketTypeDto>();

    public IEnumerable<string>? Reasons { get; set; } = new List<string>();

    public IFormFileCollection? EventSubImages { get; set; } = null;

    public CreateEmailContentDto? EmailContent { get; set; } = null;
}