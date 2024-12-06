using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.SeedWork.Entities;

namespace EventHub.Domain.Aggregates.PermissionAggregate;

[Table("Commands")]
public class Command : EntityBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [Column(TypeName = "nvarchar(50)")]
    public required string Name { get; set; } = string.Empty;

    public virtual ICollection<CommandInFunction> CommandInFunctions { get; set; } = new List<CommandInFunction>();

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}
