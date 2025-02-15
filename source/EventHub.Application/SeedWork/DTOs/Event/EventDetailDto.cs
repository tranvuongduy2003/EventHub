﻿using System.Text.Json.Serialization;
using EventHub.Application.SeedWork.DTOs.Category;
using EventHub.Application.SeedWork.DTOs.Coupon;
using EventHub.Application.SeedWork.DTOs.User;
using EventHub.Domain.Shared.Enums.Event;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Application.SeedWork.DTOs.Event;

public class EventDetailDto
{
    [SwaggerSchema("Unique identifier for the event")]
    public Guid Id { get; set; }

    [SwaggerSchema("URL for the cover image of the event")]
    public string CoverImageUrl { get; set; } = string.Empty;

    [SwaggerSchema("Name of the event")]
    public string Name { get; set; } = string.Empty;

    [SwaggerSchema("Detailed description of the event")]
    public string Description { get; set; } = string.Empty;

    [SwaggerSchema("Location where the event will take place")]
    public string Location { get; set; } = string.Empty;

    [SwaggerSchema("Start time of the event (UTC)")]
    public DateTime StartTime { get; set; } = DateTime.UtcNow;

    [SwaggerSchema("End time of the event (UTC)")]
    public DateTime EndTime { get; set; } = DateTime.UtcNow;

    [SwaggerSchema("Number of users who have marked the event as favourite")]
    public int? NumberOfFavourites { get; set; } = 0;

    [SwaggerSchema("Number of tickets sold for the event")]
    public int? NumberOfSoldTickets { get; set; } = 0;

    [SwaggerSchema("The avarage rating for this event")]
    public double AverageRate { get; set; } = 0;

    public bool IsReviewable { get; set; }

    [SwaggerSchema("Current status of the event")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EEventStatus? Status { get; set; }

    [SwaggerSchema("Type of event cycle (e.g., ONETIME, RECURRING)")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EEventCycleType EventCycleType { get; set; } = EEventCycleType.ONETIME;

    [SwaggerSchema("Payment type for the event (e.g., FREE, PAID)")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EEventPaymentType EventPaymentType { get; set; } = EEventPaymentType.FREE;

    [SwaggerSchema("Indicates if the event is private")]
    public bool IsPrivate { get; set; } = false;

    [SwaggerSchema("List of URLs for additional event images")]
    public List<string> EventSubImageUrls { get; set; } = new();

    [SwaggerSchema("List of reasons associated with the event")]
    public List<string> Reasons { get; set; } = new();

    [SwaggerSchema("Categories associated with the event")]
    public List<CategoryDto> Categories { get; set; } = new();

    [SwaggerSchema("List of ticket types available for the event")]
    public List<TicketTypeDto> TicketTypes { get; set; } = new();

    [SwaggerSchema("List of coupons available for the event")]
    public List<CouponDto> Coupons { get; set; } = new();

    [SwaggerSchema("Email content related to the event")]
    public EmailContentDto EmailContent { get; set; }

    [SwaggerSchema("Current user favourite event or not")]
    public bool IsFavourite { get; set; } = false;

    [SwaggerSchema("Author of the event")] public AuthorDto Author { get; set; }

    [SwaggerSchema("Creation timestamp of the event")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [SwaggerSchema("Timestamp of the last update to the event")]
    public DateTime? UpdatedAt { get; set; } = null;
}
