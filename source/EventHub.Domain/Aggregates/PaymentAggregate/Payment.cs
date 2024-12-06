using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.Aggregates.TicketAggregate;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.SeedWork.AggregateRoot;
using EventHub.Domain.SeedWork.Entities;
using EventHub.Domain.Shared.Enums.Payment;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.Aggregates.PaymentAggregate;

[Table("Payments")]
public class Payment : AggregateRoot, IAuditable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [Range(0, double.PositiveInfinity)]
    public required int TicketQuantity { get; set; } = 0;

    [Required]
    [MaxLength(100)]
    [Column(TypeName = "nvarchar(100)")]
    public required string CustomerName { get; set; } = string.Empty;

    [Required] 
    [MaxLength(100)] 
    [Phone] 
    public required string CustomerPhone { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public required string CustomerEmail { get; set; } = string.Empty;

    [Required]
    [Range(0, double.PositiveInfinity)]
    public required long TotalPrice { get; set; } = 0;

    [Required] 
    [Range(0.00, 1.00)] 
    public required double Discount { get; set; } = 0;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required EPaymentStatus Status { get; set; } = EPaymentStatus.PENDING;

    [Required] 
    public required Guid UserPaymentMethodId { get; set; } = Guid.Empty;

    public Guid? PaymentSessionId { get; set; } = Guid.Empty;
    
    [Required] 
    public required Guid EventId { get; set; } = Guid.Empty;
    
    [Required]
    public required Guid AuthorId { get; set; } = Guid.Empty;
    
    [ForeignKey("AuthorId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual User Author { get; set; } = null!;

    [ForeignKey("EventId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Event Event { get; set; } = null!;

    [ForeignKey("UserPaymentMethodId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual UserPaymentMethod UserPaymentMethod { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual ICollection<PaymentItem> PaymentItems { get; set; } = new List<PaymentItem>();
}
