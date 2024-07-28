﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.Common.Entities;

namespace EventHub.Domain.Entities;

[Table("PaymentMethods")]
public class PaymentMethod : EntityBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column(TypeName = "nvarchar(50)")] public string MethodName { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "nvarchar(max)")]
    public string MethodLogo { get; set; } = string.Empty;

    public virtual ICollection<UserPaymentMethod> UserPaymentMethods { get; set; } = new List<UserPaymentMethod>();
}