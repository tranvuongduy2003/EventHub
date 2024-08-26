using EventHub.Shared.DTOs.Event;

namespace EventHub.Shared.DTOs.Payment;

public class PaymentItemDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public int Quantity { get; set; } = 0;
    
    public long TotalPrice { get; set; } = 0;
    
    public double Discount { get; set; } = 0;
    
    public TicketTypeDto TicketType { get; set; } = null!;
}