using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using EventHub.Domain.Aggregates.CategoryAggregate;
using EventHub.Domain.Aggregates.ConversationAggregate;
using EventHub.Domain.Aggregates.LabelAggregate;
using EventHub.Domain.Aggregates.PaymentAggregate;
using EventHub.Domain.Aggregates.ReviewAggregate;
using EventHub.Domain.Aggregates.TicketAggregate;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.AggregateRoot;
using EventHub.Domain.SeedWork.Entities;
using EventHub.Domain.Shared.Enums.Event;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.Aggregates.EventAggregate;

[Table("Events")]
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
    [Column(TypeName = "nvarchar(max)")]
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
    public EEventCycleType EventCycleType { get; set; } = EEventCycleType.ONETIME;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EEventPaymentType EventPaymentType { get; set; } = EEventPaymentType.FREE;

    public bool IsPrivate { get; set; }

    [Required] public Guid AuthorId { get; set; } = Guid.Empty;

    public void FavouriteEvent(Guid userId, Guid eventId)
    {
        RaiseDomainEvent(new FavouriteEventDomainEvent(Guid.NewGuid(), userId, eventId));
    }

    public void UnfavouriteEvent(Guid userId, Guid eventId)
    {
        RaiseDomainEvent(new UnfavouriteEventDomainEvent(Guid.NewGuid(), userId, eventId));
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
