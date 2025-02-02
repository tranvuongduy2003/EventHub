namespace EventHub.Application.Commands.Payment.Checkout;

public class CheckoutItemCommand
{
    public string Name { get; set; }

    public Guid EventId { get; set; }

    public Guid TicketTypeId { get; set; }

    public long Price { get; set; }

    public int Quantity { get; set; }
}
