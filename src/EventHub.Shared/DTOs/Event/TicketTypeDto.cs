namespace EventHub.Shared.DTOs.Event;

public class TicketTypeDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public int Quantity { get; set; } = 0;

    public int? NumberOfSoldTickets { get; set; } = 0;

    public long Price { get; set; }
}