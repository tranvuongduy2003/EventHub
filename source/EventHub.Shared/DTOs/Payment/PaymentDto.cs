using System.Text.Json.Serialization;
using EventHub.Shared.DTOs.Event;
using EventHub.Shared.DTOs.User;
using EventHub.Shared.Enums.Payment;

namespace EventHub.Shared.DTOs.Payment;

public class PaymentDto
{
    public Guid Id { get; set; }

    public int TicketQuantity { get; set; } = 0;

    public string CustomerName { get; set; } = string.Empty;

    public string CustomerPhone { get; set; } = string.Empty;

    public string CustomerEmail { get; set; } = string.Empty;

    public decimal TotalPrice { get; set; } = 0;

    public double Discount { get; set; } = 0;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EPaymentStatus Status { get; set; }

    public EventDto? Event { get; set; } = null;
    
    public UserDto? Author { get; set; } = null;

    public UserPaymentMethodDto? UserPaymentMethod { get; set; } = null;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}