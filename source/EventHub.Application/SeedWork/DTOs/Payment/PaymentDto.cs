using System.Text.Json.Serialization;
using EventHub.Application.SeedWork.DTOs.Event;
using EventHub.Application.SeedWork.DTOs.User;
using EventHub.Domain.Shared.Enums.Payment;

namespace EventHub.Application.SeedWork.DTOs.Payment;

public class PaymentDto
{
    public Guid Id { get; set; }

    public int TicketQuantity { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public string CustomerPhone { get; set; } = string.Empty;

    public string CustomerEmail { get; set; } = string.Empty;

    public decimal TotalPrice { get; set; } = 0;

    public double Discount { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EPaymentStatus Status { get; set; }

    public EventDto? Event { get; set; }

    public UserDto? Author { get; set; }

    public UserPaymentMethodDto? UserPaymentMethod { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
