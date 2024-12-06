namespace EventHub.Domain.SeedWork.Entities;

public abstract class EntityBase : IEntityBase
{
    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
