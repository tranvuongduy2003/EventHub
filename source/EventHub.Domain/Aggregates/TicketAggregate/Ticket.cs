using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.Aggregates.EventAggregate.Entities;
using EventHub.Domain.Aggregates.PaymentAggregate;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.SeedWork.AggregateRoot;
using EventHub.Domain.Shared.Enums.Ticket;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.Aggregates.TicketAggregate;

[Table("Tickets")]
public class Ticket : AggregateRoot
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required] [MaxLength(50)] public required string TicketNo { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [Column(TypeName = "nvarchar(100)")]
    public required string CustomerName { get; set; } = string.Empty;

    [Required] [MaxLength(100)] [Phone] public required string CustomerPhone { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public required string CustomerEmail { get; set; } = string.Empty;

    [Required] public required Guid TicketTypeId { get; set; } = Guid.Empty;

    [Required] public required Guid EventId { get; set; } = Guid.Empty;

    [Required] public required Guid UserId { get; set; } = Guid.Empty;

    [Required] public required Guid PaymentId { get; set; } = Guid.Empty;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required ETicketStatus Status { get; set; } = ETicketStatus.INACTIVE;

    [ForeignKey("TicketTypeId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual TicketType TicketType { get; set; } = null!;

    [ForeignKey("EventId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Event Event { get; set; } = null!;

    [ForeignKey("AuthorId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual User User { get; set; } = null!;

    [ForeignKey("PaymentId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Payment Payment { get; set; } = null!;
}
