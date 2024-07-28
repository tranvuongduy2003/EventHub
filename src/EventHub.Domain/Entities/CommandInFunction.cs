using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.Entities;

[Table("CommandInFunctions")]
[PrimaryKey("CommandId", "FunctionId")]
public class CommandInFunction : EntityBase
{
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    [Required]
    public string CommandId { get; set; } = string.Empty;

    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    [Required]
    public string FunctionId { get; set; } = string.Empty;

    [ForeignKey("CommandId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Command Command { get; set; } = null!;

    [ForeignKey("FunctionId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Function Function { get; set; } = null!;
}