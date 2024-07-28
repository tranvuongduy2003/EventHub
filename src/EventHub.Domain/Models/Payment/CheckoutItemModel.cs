namespace EventHub.Domain.Models.Payment;

public class CheckoutItemModel
{
    public string TicketTypeId { get; set; }

    public string Name { get; set; }

    public long Price { get; set; }

    public int Quantity { get; set; }
}