namespace EventHub.Domain.SeedWork.Interfaces;

public interface ISoftDeletable
{
    DateTime? DeletedAt { get; set; }
}