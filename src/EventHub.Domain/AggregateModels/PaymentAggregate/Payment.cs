using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.AggregateModels.TicketAggregate;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.SeedWork.AggregateRoot;
using EventHub.Domain.SeedWork.Entities;
using EventHub.Domain.SeedWork.Interfaces;
using EventHub.Shared.Enums.Payment;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.AggregateModels.PaymentAggregate;

[Table("Payments")]
public class Payment : AggregateRoot, IAuditable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required] public Guid EventId { get; set; }

    [Required]
    [Range(0, double.PositiveInfinity)]
    public int TicketQuantity { get; set; } = 0;

    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public Guid UserId { get; set; } = Guid.Empty;

    [Required]
    [MaxLength(100)]
    [Column(TypeName = "nvarchar(100)")]
    public string CustomerName { get; set; } = string.Empty;

    [Required] [MaxLength(100)] [Phone] public string CustomerPhone { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string CustomerEmail { get; set; } = string.Empty;

    [Required]
    [Range(0, double.PositiveInfinity)]
    public decimal TotalPrice { get; set; } = 0;

    [Required] [Range(0.00, 1.00)] public double Discount { get; set; } = 0;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EPaymentStatus Status { get; set; }

    [Required] public Guid UserPaymentMethodId { get; set; }

    public Guid? PaymentSessionId { get; set; }
    
    public Guid AuthorId { get; set; }
    
    [ForeignKey("AuthorId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual User Author { get; set; } = null!;

    [ForeignKey("EventId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Event Event { get; set; } = null!;

    [ForeignKey("UserId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual User User { get; set; } = null!;

    [ForeignKey("UserPaymentMethodId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual UserPaymentMethod UserPaymentMethod { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual ICollection<PaymentItem> PaymentItems { get; set; } = new List<PaymentItem>();
}