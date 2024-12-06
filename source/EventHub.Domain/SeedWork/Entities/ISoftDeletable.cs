namespace EventHub.Domain.SeedWork.Entities;

public interface ISoftDeletable
{
    bool IsDeleted { get; set; }
    
    DateTime? DeletedAt { get; set; }
}
