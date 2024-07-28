using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.Entities;

[Table("UserPaymentMethods")]
public class UserPaymentMethod : EntityBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Uuid { get; set; }

    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string UserId { get; set; } = string.Empty;

    [Required] public int MethodId { get; set; }

    [Required] [MaxLength(50)] public string PaymentAccountNumber { get; set; } = string.Empty;

    [Column(TypeName = "nvarchar(max)")] public string? PaymentAccountQRCode { get; set; }

    [MaxLength(200)]
    [Column(TypeName = "nvarchar(200)")]
    public string? CheckoutContent { get; set; } = string.Empty;

    [ForeignKey("UserId")]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual User User { get; set; } = null!;

    [ForeignKey("MethodId")]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual PaymentMethod Method { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}