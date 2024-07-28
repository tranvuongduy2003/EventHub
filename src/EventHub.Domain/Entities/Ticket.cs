using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using EventHub.Domain.Common.Entities;
using EventHub.Domain.Enums.Ticket;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.Entities;

[Table("Tickets")]
public class Ticket : EntityBase
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

    [Required] public int TicketTypeId { get; set; }

    [Required] public int EventId { get; set; }

    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string UserId { get; set; } = string.Empty;

    [Required] public int PaymentId { get; set; }

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