using EventHub.Domain.Common.Interfaces;
using EventHub.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace EventHub.Domain.Common.Entities;

public class Role : IdentityRole, IDateTracking, ISoftDeletable
{
    public bool IsDeleted { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    public DateTime? DeletedAt { get; set; }
    
    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}