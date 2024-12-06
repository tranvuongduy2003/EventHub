using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.SeedWork.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.Aggregates.PermissionAggregate;

[Table("CommandInFunctions")]
[PrimaryKey("CommandId", "FunctionId")]
public class CommandInFunction : EntityBase
{
    [Required]
    [MaxLength(50)]
    public required string CommandId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public required string FunctionId { get; set; } = string.Empty;

    [ForeignKey("CommandId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Command Command { get; set; } = null!;

    [ForeignKey("FunctionId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Function Function { get; set; } = null!;
}
