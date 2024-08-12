using System.Text.Json.Serialization;
using EventHub.Shared.Enums.Payment;

namespace EventHub.Shared.DTOs.Payment;

public class CreatePaymentDto
{
    public string EventId { get; set; }

    public int TicketQuantity { get; set; } = 0;

    public string UserId { get; set; }

    public string CustomerName { get; set; }

    public string CustomerPhone { get; set; }

    public string CustomerEmail { get; set; }

    public decimal TotalPrice { get; set; } = 0;

    public double Discount { get; set; } = 0;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EPaymentStatus Status { get; set; }

    public string UserPaymentMethodId { get; set; }

    public string PaymentSessionId { get; set; }
}