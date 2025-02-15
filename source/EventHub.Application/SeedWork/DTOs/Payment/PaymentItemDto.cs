﻿using EventHub.Application.SeedWork.DTOs.Event;

namespace EventHub.Application.SeedWork.DTOs.Payment;

public class PaymentItemDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public long TotalPrice { get; set; }

    public EventDto Event { get; set; } = null!;

    public TicketTypeDto TicketType { get; set; } = null!;
}
