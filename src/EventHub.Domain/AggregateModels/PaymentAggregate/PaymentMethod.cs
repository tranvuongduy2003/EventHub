using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.SeedWork.Entities;

namespace EventHub.Domain.AggregateModels.PaymentAggregate;

[Table("PaymentMethods")]
public class PaymentMethod : EntityBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [MaxLength(50)]
    [Column(TypeName = "nvarchar(50)")]
    public required string MethodName { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column(TypeName = "nvarchar(255)")]
    public required string MethodLogoFileName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(255)]
    [Column(TypeName = "nvarchar(255)")]
    public required string MethodLogoUrl { get; set; } = string.Empty;

    public virtual ICollection<UserPaymentMethod> UserPaymentMethods { get; set; } = new List<UserPaymentMethod>();
}