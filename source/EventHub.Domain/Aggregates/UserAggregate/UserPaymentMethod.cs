using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.Aggregates.PaymentAggregate;
using EventHub.Domain.SeedWork.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.Aggregates.UserAggregate;

[Table("UserPaymentMethods")]
public class UserPaymentMethod : EntityBase, IAuditable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public required Guid AuthorId { get; set; } = Guid.Empty;

    [Required]
    public required Guid PaymentMethodId { get; set; } = Guid.Empty;

    [Required]
    [MaxLength(50)]
    public required string PaymentAccountNumber { get; set; } = string.Empty;

    [Column(TypeName = "nvarchar(max)")]
    public string? PaymentAccountQrCodeUrl { get; set; }

    [MaxLength(255)]
    [Column(TypeName = "nvarchar(255)")]
    public string? PaymentAccountQrCodeFileName { get; set; }

    [MaxLength(200)]
    [Column(TypeName = "nvarchar(200)")]
    public string? CheckoutContent { get; set; } = string.Empty;

    [ForeignKey("AuthorId")]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual User Author { get; set; } = null!;

    [ForeignKey("PaymentMethodId")]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual PaymentMethod PaymentMethod { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
