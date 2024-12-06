namespace EventHub.Application.DTOs.Payment;

public class CheckoutItemDto
{
    public string TicketTypeId { get; set; }

    public string Name { get; set; }

    public long Price { get; set; }

    public int Quantity { get; set; }
}
