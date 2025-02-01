namespace EventHub.Application.Commands.Payment.Checkout;

public class CheckoutItemCommand
{
    public Guid EventId { get; set; }

    public Guid TicketTypeId { get; set; }

    public long TotalPrice { get; set; }

    public int Quantity { get; set; }
}
