using EventHub.Domain.AggregateModels.PermissionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHub.Persistence.Configurations;

public class PermissionAggregateRootConfiguration: IEntityTypeConfiguration<PermissionAggregateRoot>
{
    public void Configure(EntityTypeBuilder<PermissionAggregateRoot> builder)
    {
        builder.HasNoKey();
    }
}