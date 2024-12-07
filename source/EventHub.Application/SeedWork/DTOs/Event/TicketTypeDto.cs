using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Application.SeedWork.DTOs.Event;

public class TicketTypeDto
{
    [SwaggerSchema("Unique identifier for the ticket type")]
    public Guid Id { get; set; }

    [SwaggerSchema("Name of the ticket type")]
    public string Name { get; set; } = string.Empty;

    [SwaggerSchema("Available quantity of the ticket type")]
    public int Quantity { get; set; } = 0;

    [SwaggerSchema("Number of tickets sold for this ticket type")]
    public int? NumberOfSoldTickets { get; set; } = 0;

    [SwaggerSchema("Price of the ticket in cents")]
    public long Price { get; set; } = 0;
}
