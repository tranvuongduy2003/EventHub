using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.Entities;

[Table("Functions")]
public class Function : EntityBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    [Column(TypeName = "nvarchar(200)")]
    public string Name { get; set; } = string.Empty;

    [Required] [MaxLength(200)] public string Url { get; set; } = string.Empty;

    [Required] public int SortOrder { get; set; } = 0;

    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string? ParentId { get; set; }

    [ForeignKey("ParentId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Function Parent { get; set; } = null!;

    public virtual ICollection<CommandInFunction> CommandInFunctions { get; set; } = new List<CommandInFunction>();

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}