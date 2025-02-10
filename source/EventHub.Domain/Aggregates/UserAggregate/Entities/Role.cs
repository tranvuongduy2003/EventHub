using EventHub.Domain.Aggregates.UserAggregate.ValueObjects;
using EventHub.Domain.SeedWork.Entities;
using Microsoft.AspNetCore.Identity;

namespace EventHub.Domain.Aggregates.UserAggregate.Entities;

public class Role : IdentityRole<Guid>, IDateTracking, ISoftDeletable
{
    public Role(string name) : base(name)
    {
    }

    public string? Description { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}
