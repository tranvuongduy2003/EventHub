using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.SeedWork.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.AggregateModels.PermissionAggregate;

[Table("Functions")]
public class Function : EntityBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    [Column(TypeName = "nvarchar(200)")]
    public required string Name { get; set; } = string.Empty;

    [Required] 
    [MaxLength(200)]
    public required string Url { get; set; } = string.Empty;

    [Required] 
    public required int SortOrder { get; set; } = 0;

    [MaxLength(50)]
    public string? ParentId { get; set; }

    [ForeignKey("ParentId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Function Parent { get; set; } = null!;

    public virtual ICollection<CommandInFunction> CommandInFunctions { get; set; } = new List<CommandInFunction>();

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}