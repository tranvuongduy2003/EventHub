using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.AggregateModels.TicketAggregate;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.SeedWork.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.AggregateModels.PaymentAggregate;

[Table("PaymentItems")]
public class PaymentItem : EntityBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required] public int EventId { get; set; }

    [Required] public int TicketTypeId { get; set; }

    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string UserId { get; set; } = string.Empty;

    [Required] public int PaymentId { get; set; }

    [Required]
    [MaxLength(100)]
    [Column(TypeName = "nvarchar(100)")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(0, double.PositiveInfinity)]
    public int Quantity { get; set; } = 0;

    [Required] [Range(0, 1000000000)] public long TotalPrice { get; set; } = 0;

    [Required] [Range(0.00, 1.00)] public double Discount { get; set; } = 0;

    [ForeignKey("EventId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Event Event { get; set; } = null!;

    [ForeignKey("UserId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual User User { get; set; } = null!;

    [ForeignKey("PaymentId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Payment Payment { get; set; } = null!;

    [ForeignKey("TicketTypeId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual TicketType TicketType { get; set; } = null!;
}