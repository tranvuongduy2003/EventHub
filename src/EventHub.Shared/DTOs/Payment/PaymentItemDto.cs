namespace EventHub.Shared.DTOs.Payment;

public class PaymentItemDto
{
    public string Id { get; set; }

    public string EventId { get; set; }

    public string EventName { get; set; }

    public string TicketTypeId { get; set; }

    public string TicketTypeName { get; set; }

    public string UserId { get; set; }

    public string PaymentId { get; set; }

    public long TotalPrice { get; set; } = 0;

    public int Quantity { get; set; } = 0;

    public double Discount { get; set; } = 0;
}