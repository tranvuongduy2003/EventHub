namespace EventHub.Domain.Common.Interfaces;

public interface IEntityAuditBase : IEntityBase
{
    string AuthorId { get; set; }
}