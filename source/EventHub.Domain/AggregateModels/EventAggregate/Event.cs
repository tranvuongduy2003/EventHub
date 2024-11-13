using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using EventHub.Domain.AggregateModels.CategoryAggregate;
using EventHub.Domain.AggregateModels.ConversationAggregate;
using EventHub.Domain.AggregateModels.LabelAggregate;
using EventHub.Domain.AggregateModels.PaymentAggregate;
using EventHub.Domain.AggregateModels.ReviewAggregate;
using EventHub.Domain.AggregateModels.TicketAggregate;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.AggregateRoot;
using EventHub.Domain.SeedWork.Interfaces;
using EventHub.Shared.DTOs.Event;
using EventHub.Shared.Enums.Event;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.AggregateModels.EventAggregate;

[Table("events")]
public class Event : AggregateRoot, IAuditable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(255)]
    [Column(TypeName = "nvarchar(255)")]
    public string CoverImageFileName { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column(TypeName = "nvarchar(255)")]
    public string CoverImageUrl { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [Column(TypeName = "nvarchar(100)")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    [Column(TypeName = "nvarchar(1000)")]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    [Column(TypeName = "nvarchar(1000)")]
    public string Location { get; set; } = string.Empty;

    [Required] public DateTime StartTime { get; set; } = DateTime.UtcNow;

    [Required] public DateTime EndTime { get; set; } = DateTime.UtcNow;

    [Range(0.0, 1.0)] public double? Promotion { get; set; } = 0;

    [Range(0, double.PositiveInfinity)] public int? NumberOfFavourites { get; set; } = 0;

    [Range(0, double.PositiveInfinity)] public int? NumberOfShares { get; set; } = 0;

    [Range(0, double.PositiveInfinity)] public int? NumberOfSoldTickets { get; set; } = 0;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EEventStatus? Status { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EEventCycleType EventCycleType { get; set; } = EEventCycleType.SINGLE;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EEventPaymentType EventPaymentType { get; set; } = EEventPaymentType.FREE;

    public bool IsPrivate { get; set; } = false;

    [Required] public Guid AuthorId { get; set; } = Guid.Empty;

    public static async Task UploadAndSaveEventSubImages(Guid eventId, IFormFileCollection subImages)
    {
        new Event().RaiseDomainEvent(
            new UploadAndSaveEventSubImagesDomainEvent(Guid.NewGuid(), eventId, subImages));
    }

    public static async Task DeleteEventSubImages(Guid eventId)
    {
        new Event().RaiseDomainEvent(
            new DeleteEventSubImagesDomainEvent(Guid.NewGuid(), eventId));
    }

    public static async Task CreateEmailContentOfEvent(Guid eventId, CreateEmailContentDto emailContent)
    {
        new Event().RaiseDomainEvent(
            new CreateEmailContentOfEventDomainEvent(Guid.NewGuid(), eventId, emailContent));
    }

    public static async Task UpdateEmailContentOfEvent(Guid eventId, UpdateEmailContentDto emailContent)
    {
        new Event().RaiseDomainEvent(
            new UpdateEmailContentOfEventDomainEvent(Guid.NewGuid(), eventId, emailContent));
    }

    public static async Task DeleteEmailContent(Guid eventId)
    {
        new Event().RaiseDomainEvent(
            new DeleteEmailContentDomainEvent(Guid.NewGuid(), eventId));
    }

    public static async Task CreateTicketTypesOfEvent(Guid eventId, List<CreateTicketTypeDto> ticketTypes)
    {
        new Event().RaiseDomainEvent(
            new CreateTicketTypesOfEventDomainEvent(Guid.NewGuid(), eventId, ticketTypes));
    }

    public static async Task UpdateTicketTypesInEvent(Guid eventId, List<UpdateTicketTypeDto> ticketTypes)
    {
        new Event().RaiseDomainEvent(
            new UpdateTicketTypesInEventDomainEvent(Guid.NewGuid(), eventId, ticketTypes));
    }

    public static async Task DeleteEventTicketTypes(Guid eventId)
    {
        new Event().RaiseDomainEvent(
            new DeleteEventTicketTypesDomainEvent(Guid.NewGuid(), eventId));
    }

    public static async Task AddEventToCategories(Guid eventId, List<Guid> categories)
    {
        new Event().RaiseDomainEvent(
            new AddEventToCategoriesDomainEvent(Guid.NewGuid(), eventId, categories));
    }

    public static async Task UpdateCategoriesInEvent(Guid eventId, List<Guid> categories)
    {
        new Event().RaiseDomainEvent(
            new UpdateCategoriesInEventDomainEvent(Guid.NewGuid(), eventId, categories));
    }

    public static async Task RemoveEventFromCategories(Guid eventId)
    {
        new Event().RaiseDomainEvent(
            new RemoveEventFromCategoriesDomainEvent(Guid.NewGuid(), eventId));
    }

    public static async Task CreateReasonsToRegisterEvent(Guid eventId, List<string> reasons)
    {
        new Event().RaiseDomainEvent(
            new CreateReasonsToRegisterEventDomainEvent(Guid.NewGuid(), eventId, reasons));
    }

    public static async Task UpdateReasonsInEvent(Guid eventId, List<string> reasons)
    {
        new Event().RaiseDomainEvent(
            new UpdateReasonsInEventDomainEvent(Guid.NewGuid(), eventId, reasons));
    }

    public static async Task DeleteEventReasons(Guid eventId)
    {
        new Event().RaiseDomainEvent(
            new DeleteEventReasonsDomainEvent(Guid.NewGuid(), eventId));
    }

    public static async Task FavouriteEvent(Guid userId, Guid eventId)
    {
        new Event().RaiseDomainEvent(
            new FavouriteEventDomainEvent(Guid.NewGuid(), userId, eventId));
    }

    public static async Task UnfavouriteEvent(Guid userId, Guid eventId)
    {
        new Event().RaiseDomainEvent(
            new UnfavouriteEventDomainEvent(Guid.NewGuid(), userId, eventId));
    }

    public static async Task MakeEventsPrivate(Guid userId, List<Guid> events)
    {
        new Event().RaiseDomainEvent(
            new MakeEventsPrivateDomainEvent(Guid.NewGuid(), userId, events));
    }

    public static async Task MakeEventsPublic(Guid userId, List<Guid> events)
    {
        new Event().RaiseDomainEvent(
            new MakeEventsPublicDomainEvent(Guid.NewGuid(), userId, events));
    }

    public static async Task RestoreEvent(Guid userId, List<Guid> events)
    {
        new Event().RaiseDomainEvent(
            new RestoreEventDomainEvent(Guid.NewGuid(), userId, events));
    }

    #region Relationships

    [ForeignKey("AuthorId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual User Author { get; set; } = null!;

    public virtual EmailContent? EmailContent { get; set; }

    public virtual ICollection<EventCategory> EventCategories { get; set; } = new List<EventCategory>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual ICollection<LabelInEvent> LabelInEvents { get; set; } = new List<LabelInEvent>();

    public virtual ICollection<FavouriteEvent> FavouriteEvents { get; set; } = new List<FavouriteEvent>();

    public virtual ICollection<EventSubImage> EventSubImages { get; set; } = new List<EventSubImage>();

    public virtual ICollection<TicketType> TicketTypes { get; set; } = new List<TicketType>();

    public virtual ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual ICollection<PaymentItem> PaymentItems { get; set; } = new List<PaymentItem>();

    public virtual ICollection<Reason> Reasons { get; set; } = new List<Reason>();

    #endregion
}