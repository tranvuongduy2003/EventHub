using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.SeedWork.AggregateRoot;

namespace EventHub.Domain.Aggregates.CategoryAggregate;

[Table("Categories")]
public class Category : AggregateRoot
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column(TypeName = "nvarchar(100)")]
    public required string Name { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "nvarchar(max)")]
    public required string IconImageUrl { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column(TypeName = "nvarchar(255)")]
    public required string IconImageFileName { get; set; } = string.Empty;

    [Required][MaxLength(50)] public required string Color { get; set; } = "#FFFFFF";

    public virtual ICollection<EventCategory> EventCategories { get; set; } = new List<EventCategory>();
}
