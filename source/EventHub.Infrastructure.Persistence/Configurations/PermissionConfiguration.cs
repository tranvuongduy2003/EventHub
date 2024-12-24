using EventHub.Domain.Aggregates.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHub.Infrastructure.Persistence.Configurations;

public class PermissionConfiguration: IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder
            .HasOne(x => x.Function)
            .WithMany(x => x.Permissions)
            .HasForeignKey(x => x.FunctionId);
        
        builder
            .HasOne(x => x.Role)
            .WithMany(x => x.Permissions)
            .HasForeignKey(x => x.RoleId);
        
        builder
            .HasOne(x => x.Command)
            .WithMany(x => x.Permissions)
            .HasForeignKey(x => x.CommandId);

    }
}