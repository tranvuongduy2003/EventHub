using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.Entities;

[Table("Reasons")]
public class Reason : EntityBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required] public int EventId { get; set; }

    [Required]
    [MaxLength(1000)]
    [Column(TypeName = "nvarchar(1000)")]
    public string Name { get; set; } = string.Empty;

    [ForeignKey("EventId")]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual Event Event { get; set; } = null!;
}