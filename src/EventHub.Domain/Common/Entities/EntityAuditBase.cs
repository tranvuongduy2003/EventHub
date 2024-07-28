using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.Common.Entities;

public class EntityAuditBase : EntityBase, IEntityAuditBase
{
    [ForeignKey("AuthorId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual User Author { get; set; } = null!;

    public string AuthorId { get; set; }
}