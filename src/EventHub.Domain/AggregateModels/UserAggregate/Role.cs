using EventHub.Domain.AggregateModels.PermissionAggregate;
using EventHub.Domain.SeedWork.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace EventHub.Domain.AggregateModels.UserAggregate;

public class Role : IdentityRole, IDateTracking, ISoftDeletable
{
    public bool IsDeleted { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    public DateTime? DeletedAt { get; set; }
    
    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}