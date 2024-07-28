namespace EventHub.Domain.DTOs.Event;

public class CreateTicketTypeDto
{
    public string Name { get; set; }

    public int Quantity { get; set; } = 0;

    public long Price { get; set; } = 0;
}