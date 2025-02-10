using System.Text.Json.Serialization;
using EventHub.Application.SeedWork.DTOs.Ticket;
using EventHub.Domain.Shared.Enums.Payment;

namespace EventHub.Application.SeedWork.DTOs.Payment;

public class ValidateSessionResponseDto
{
    public Guid Id { get; set; }

    public string CustomerName { get; set; }

    public string CustomerPhone { get; set; }

    public string CustomerEmail { get; set; }

    public decimal TotalPrice { get; set; }

    public int TicketQuantity { get; set; }

    public double Discount { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EPaymentStatus Status { get; set; }

    public Guid? OrganizerId { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public List<TicketDto> Tickets { get; set; }
}
