using EventHub.Domain.Aggregates.PermissionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHub.Infrastructure.Persistence.Configurations;

public class PermissionAggregateRootConfiguration: IEntityTypeConfiguration<PermissionAggregateRoot>
{
    public void Configure(EntityTypeBuilder<PermissionAggregateRoot> builder)
    {
        builder.HasNoKey();
    }
}