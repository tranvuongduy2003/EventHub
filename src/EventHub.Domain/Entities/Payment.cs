﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using EventHub.Domain.Common.Entities;
using EventHub.Domain.Enums.Payment;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.Entities;

[Table("Payments")]
public class Payment : EntityAuditBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Uuid { get; set; }

    [Required] public int EventId { get; set; }

    [Required]
    [Range(0, double.PositiveInfinity)]
    public int TicketQuantity { get; set; } = 0;

    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string UserId { get; set; } = string.Empty;

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

    [Required] public int UserPaymentMethodId { get; set; }

    public string? PaymentSessionId { get; set; }

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