using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.AggregateModels.PaymentAggregate;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.SeedWork.AggregateRoot;
using EventHub.Domain.SeedWork.Entities;
using EventHub.Shared.Enums.Ticket;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.AggregateModels.TicketAggregate;

[Table("Tickets")]
public class Ticket : AggregateRoot
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string TicketNo { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [Column(TypeName = "nvarchar(100)")]
    public string CustomerName { get; set; } = string.Empty;

    [Required] [MaxLength(100)] [Phone] public string CustomerPhone { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string CustomerEmail { get; set; } = string.Empty;

    [Required] public Guid TicketTypeId { get; set; }

    [Required] public Guid EventId { get; set; }

    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public Guid UserId { get; set; } = Guid.Empty;

    [Required] public Guid PaymentId { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ETicketStatus Status { get; set; } = ETicketStatus.INACTIVE;

    [ForeignKey("TicketTypeId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual TicketType TicketType { get; set; } = null!;

    [ForeignKey("EventId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Event Event { get; set; } = null!;

    [ForeignKey("UserId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual User User { get; set; } = null!;

    [ForeignKey("PaymentId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Payment Payment { get; set; } = null!;
}