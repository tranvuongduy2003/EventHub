using EventHub.Domain.AggregateModels.PermissionAggregate;
using EventHub.Domain.SeedWork.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace EventHub.Domain.AggregateModels.UserAggregate;

public class Role : IdentityRole<Guid>, IDateTracking, ISoftDeletable
{
    public Role(string name) : base(name)
    {
    }
    
    public bool IsDeleted { get; set; } = false;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    public DateTime? DeletedAt { get; set; }
    
    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}