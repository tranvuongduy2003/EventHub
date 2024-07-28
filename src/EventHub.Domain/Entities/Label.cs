using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.Common.Entities;

namespace EventHub.Domain.Entities;

[Table("Labels")]
public class Label : EntityBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    [Column(TypeName = "nvarchar(50)")]
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<LabelInEvent> LabelInEvents { get; set; } = new List<LabelInEvent>();

    public virtual ICollection<LabelInUser> LabelInUsers { get; set; } = new List<LabelInUser>();
}