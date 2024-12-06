using System.Text.Json.Serialization;
using EventHub.Domain.Shared.Enums.Payment;

namespace EventHub.Application.DTOs.Payment;

public class CreatePaymentDto
{
    public string EventId { get; set; }

    public int TicketQuantity { get; set; }

    public string UserId { get; set; }

    public string CustomerName { get; set; }

    public string CustomerPhone { get; set; }

    public string CustomerEmail { get; set; }

    public decimal TotalPrice { get; set; } = 0;

    public double Discount { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EPaymentStatus Status { get; set; }

    public string UserPaymentMethodId { get; set; }

    public string PaymentSessionId { get; set; }
}
