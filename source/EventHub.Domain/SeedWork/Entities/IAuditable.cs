namespace EventHub.Domain.SeedWork.Entities;

public interface IAuditable
{
    Guid AuthorId { get; set; }
}
