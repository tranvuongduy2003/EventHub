using System.Text.Json.Serialization;
using EventHub.Domain.Enums.Ticket;

namespace EventHub.Domain.Models.Ticket;

public class TicketModel
{
    public string Id { get; set; }

    public string CustomerName { get; set; }

    public string CustomerPhone { get; set; }

    public string CustomerEmail { get; set; }

    public string TicketTypeId { get; set; }

    public string EventId { get; set; }

    public string PaymentId { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ETicketStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}