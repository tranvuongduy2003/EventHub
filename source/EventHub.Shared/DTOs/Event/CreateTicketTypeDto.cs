using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.Event;

public class CreateTicketTypeDto
{
    [SwaggerSchema("Name of the ticket type")]
    [DefaultValue("General Admission")]
    public string Name { get; set; } = string.Empty;

    [SwaggerSchema("Available quantity of the tickets")]
    [DefaultValue(100)]
    public int Quantity { get; set; } = 0;

    [SwaggerSchema("Price of the ticket in cents")]
    [DefaultValue(5000)] // Assuming the price is in cents (e.g., 5000 = $50)
    public long Price { get; set; } = 0;
}