using System.Text.Json.Serialization;
using EventHub.Application.SeedWork.DTOs.Event;
using EventHub.Domain.Shared.Enums.Ticket;

namespace EventHub.Application.SeedWork.DTOs.Ticket;

public class TicketDto
{
    public string Id { get; set; }

    public string TicketNo { get; set; }

    public string CustomerName { get; set; }

    public string CustomerPhone { get; set; }

    public string CustomerEmail { get; set; }

    public TicketTypeDto TicketType { get; set; }

    public EventDto Event { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ETicketStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
}
