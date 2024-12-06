namespace EventHub.Domain.SeedWork.Entities;

public interface IDateTracking
{
    DateTime CreatedAt { get; set; }

    DateTime? UpdatedAt { get; set; }
}
