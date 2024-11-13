using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.AggregateModels.TicketAggregate;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.SeedWork.Entities;
using EventHub.Domain.SeedWork.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.AggregateModels.PaymentAggregate;

[Table("PaymentItems")]
public class PaymentItem : EntityBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required] 
    public required Guid TicketTypeId { get; set; } = Guid.Empty;

    [Required] 
    public required Guid PaymentId { get; set; } = Guid.Empty;

    [Required]
    [MaxLength(100)]
    [Column(TypeName = "nvarchar(100)")]
    public required string Name { get; set; } = string.Empty;

    [Required]
    [Range(0, double.PositiveInfinity)]
    public required int Quantity { get; set; } = 0;

    [Required] 
    [Range(0, 1000000000)] 
    public required long TotalPrice { get; set; } = 0;

    [Required] 
    [Range(0.00, 1.00)] 
    public required double Discount { get; set; } = 0;

    [ForeignKey("PaymentId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Payment Payment { get; set; } = null!;

    [ForeignKey("TicketTypeId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual TicketType TicketType { get; set; } = null!;
}