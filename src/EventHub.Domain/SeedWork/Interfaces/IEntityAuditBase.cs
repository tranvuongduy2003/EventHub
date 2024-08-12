namespace EventHub.Domain.SeedWork.Interfaces;

public interface IEntityAuditBase : IEntityBase
{
    string AuthorId { get; set; }
}