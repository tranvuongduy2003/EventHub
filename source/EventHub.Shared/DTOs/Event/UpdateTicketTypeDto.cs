using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.Event;

public class UpdateTicketTypeDto
{
    [SwaggerSchema("Unique identifier for the ticket type (optional, for existing tickets)")]
    public Guid? Id { get; set; }

    [SwaggerSchema("Name of the ticket type")]
    public string Name { get; set; } = string.Empty;

    [SwaggerSchema("Available quantity of the tickets")]
    public int Quantity { get; set; } = 0;

    [SwaggerSchema("Price of the ticket in cents")]
    public long Price { get; set; } = 0;
}
