using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.Aggregates.UserAggregate;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.SeedWork.Entities;

public abstract class EntityAuditBase : EntityBase, IAuditable
{
    [Required]
    public required Guid AuthorId { get; set; }
    
    [ForeignKey("AuthorId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual User Author { get; set; } = null!;
}