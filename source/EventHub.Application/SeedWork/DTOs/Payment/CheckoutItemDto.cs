namespace EventHub.Application.SeedWork.DTOs.Payment;

public class CheckoutItemDto
{
    public string Name { get; set; }

    public Guid EventId { get; set; }

    public Guid TicketTypeId { get; set; }

    public long TotalPrice { get; set; }

    public int Quantity { get; set; }
}
