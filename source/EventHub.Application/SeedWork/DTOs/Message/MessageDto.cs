﻿using EventHub.Application.SeedWork.DTOs.User;

namespace EventHub.Application.SeedWork.DTOs.Message;

public class MessageDto
{
    public Guid Id { get; set; }

    public string? Content { get; set; }

    public string? ImageUrl { get; set; } = string.Empty;

    public string? VideoUrl { get; set; } = string.Empty;

    public string? AudioUrl { get; set; } = string.Empty;

    public AuthorDto? Sender { get; set; }

    public AuthorDto? Receiver { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
