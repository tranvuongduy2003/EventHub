using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.Event;

public class TicketTypeDto
{
    [SwaggerSchema("Unique identifier for the ticket type")]
    [DefaultValue("c1d2e3f4-5a6b-7c8d-9e0f-1a2b3c4d5e6f")]
    public Guid Id { get; set; }

    [SwaggerSchema("Name of the ticket type")]
    [DefaultValue("VIP Ticket")]
    public string Name { get; set; } = string.Empty;

    [SwaggerSchema("Available quantity of the ticket type")]
    [DefaultValue(100)]
    public int Quantity { get; set; } = 0;

    [SwaggerSchema("Number of tickets sold for this ticket type")]
    [DefaultValue(0)]
    public int? NumberOfSoldTickets { get; set; } = 0;

    [SwaggerSchema("Price of the ticket in cents")]
    [DefaultValue(15000)] // Assuming the price is in cents (e.g., 15000 = $150)
    public long Price { get; set; } = 0;
}