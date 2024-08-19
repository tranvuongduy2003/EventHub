using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.SeedWork.Entities;

namespace EventHub.Domain.AggregateModels.LabelAggregate;

[Table("Labels")]
public class Label : EntityBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }

    [Required]
    [MaxLength(50)]
    [Column(TypeName = "nvarchar(50)")]
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<LabelInEvent> LabelInEvents { get; set; } = new List<LabelInEvent>();

    public virtual ICollection<LabelInUser> LabelInUsers { get; set; } = new List<LabelInUser>();
}