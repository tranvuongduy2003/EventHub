namespace EventHub.Application.SeedWork.DTOs.Payment;

public class CreateCheckoutItemDto
{
    public string TicketTypeId { get; set; }

    public string Name { get; set; }

    public long Price { get; set; }

    public int Quantity { get; set; }
}
