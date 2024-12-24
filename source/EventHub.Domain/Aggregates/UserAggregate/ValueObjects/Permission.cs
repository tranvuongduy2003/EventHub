using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.Aggregates.UserAggregate.Entities;
using EventHub.Domain.SeedWork.AggregateRoot;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.Aggregates.UserAggregate.ValueObjects;

[Table("Permissions")]
[PrimaryKey("FunctionId", "RoleId", "CommandId")]
public class Permission : AggregateRoot
{
    [MaxLength(50)]
    public required string FunctionId { get; set; } = string.Empty;
    
    public required Guid RoleId { get; set; } = Guid.Empty;

    [MaxLength(50)]
    public required string CommandId { get; set; } = string.Empty;

    [ForeignKey("FunctionId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Function Function { get; set; } = null!;

    [ForeignKey("RoleId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Role Role { get; set; } = null!;

    [ForeignKey("CommandId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Command Command { get; set; } = null!;
}
