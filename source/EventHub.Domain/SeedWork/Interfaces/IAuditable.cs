namespace EventHub.Domain.SeedWork.Interfaces;

public interface IAuditable
{
    Guid AuthorId { get; set; }
}