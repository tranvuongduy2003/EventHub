using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.Common.Entities;

namespace EventHub.Domain.Entities;

[Table("Categories")]
public class Category : EntityBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column(TypeName = "nvarchar(100)")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "nvarchar(max)")]
    public string IconImage { get; set; } = string.Empty;

    [Required] [MaxLength(50)] public string Color { get; set; } = "#FFFFFF";

    public virtual ICollection<EventCategory> EventCategories { get; set; } = new List<EventCategory>();
}