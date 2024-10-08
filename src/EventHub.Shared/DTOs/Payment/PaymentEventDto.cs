﻿namespace EventHub.Shared.DTOs.Payment;

public class PaymentEventDto
{
    public Guid Id { get; set; }

    public string CoverImageUrl { get; set; }

    public string Name { get; set; }

    public string AuthorId { get; set; }
}