namespace EventHub.Domain.SeedWork.Interfaces;

public interface IDateTracking
{
    DateTime CreatedAt { get; set; }

    DateTime? UpdatedAt { get; set; }
}