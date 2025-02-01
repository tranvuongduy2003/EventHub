using System.Text.Json.Serialization;
using EventHub.Application.SeedWork.DTOs.Event;
using EventHub.Application.SeedWork.DTOs.User;
using EventHub.Domain.Shared.Enums.Payment;

namespace EventHub.Application.SeedWork.DTOs.Payment;

public class PaymentDto
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

    public EventDto? Event { get; set; }

    public AuthorDto? Author { get; set; }

    public string? PaymentMethod { get; set; }

    public string? PaymentIntentId { get; set; }

    public string? SessionId { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<PaymentItemDto> PaymentItems { get; set; }
}
