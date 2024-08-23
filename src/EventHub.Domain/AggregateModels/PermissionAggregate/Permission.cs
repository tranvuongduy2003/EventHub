using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.AggregateRoot;
using EventHub.Domain.SeedWork.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.AggregateModels.PermissionAggregate;

[Table("Permissions")]
[PrimaryKey("FunctionId", "RoleId", "CommandId")]
public class Permission : AggregateRoot
{
    public Permission()
    {
    }
    
    public Permission(string functionId, Guid roleId, string commandId)
    {
        FunctionId = functionId;
        RoleId = roleId;
        CommandId = commandId;
    }
    
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string FunctionId { get; set; } = string.Empty;

    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public Guid RoleId { get; set; } = Guid.Empty;

    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string CommandId { get; set; } = string.Empty;

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