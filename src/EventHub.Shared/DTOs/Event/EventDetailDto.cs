using System.Text.Json.Serialization;
using EventHub.Shared.DTOs.Category;
using EventHub.Shared.DTOs.User;
using EventHub.Shared.Enums.Event;

namespace EventHub.Shared.DTOs.Event;

public class EventDetailDto
{
    public Guid Id { get; set; }
    
    public string CoverImageUrl { get; set; } = string.Empty;
    
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public string Location { get; set; } = string.Empty;

    public DateTime StartTime { get; set; } = DateTime.UtcNow;

    public DateTime EndTime { get; set; } = DateTime.UtcNow;

    public double? Promotion { get; set; } = 0;

    public int? NumberOfFavourites { get; set; } = 0;

    public int? NumberOfShares { get; set; } = 0;

    public int? NumberOfSoldTickets { get; set; } = 0;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EEventStatus? Status { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EEventCycleType EventCycleType { get; set; } = EEventCycleType.SINGLE;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EEventPaymentType EventPaymentType { get; set; } = EEventPaymentType.FREE;

    public bool IsPrivate { get; set; } = false;
    
    public List<string> EventSubImageUrls { get; set; } = new();

    public List<ReasonDto> Reasons { get; set; } = new();
    
    public List<CategoryDto> Categories { get; set; } = new();
    
    public List<TicketTypeDto> TicketTypes { get; set; }
    
    public EmailContentDto EmailContent { get; set; }

    public AuthorDto Author { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
