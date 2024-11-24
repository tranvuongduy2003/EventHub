using EventHub.Shared.DTOs.Event;

namespace EventHub.Shared.DTOs.Payment;

public class PaymentItemDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public long TotalPrice { get; set; }

    public double Discount { get; set; }

    public TicketTypeDto TicketType { get; set; } = null!;
}
