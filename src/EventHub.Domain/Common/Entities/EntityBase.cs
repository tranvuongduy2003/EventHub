using EventHub.Domain.Common.Interfaces;

namespace EventHub.Domain.Common.Entities;

public class EntityBase : IEntityBase
{
    public bool IsDeleted { get; set; } = false;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }


    public DateTime? DeletedAt { get; set; } = null;
}