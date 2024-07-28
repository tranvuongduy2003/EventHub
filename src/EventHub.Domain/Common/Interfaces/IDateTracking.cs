namespace EventHub.Domain.Common.Interfaces;

public interface IDateTracking
{
    DateTime CreatedAt { get; set; }

    DateTime? UpdatedAt { get; set; }
}