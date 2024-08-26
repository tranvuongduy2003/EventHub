using EventHub.Domain.AggregateModels.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHub.Persistence.Configurations;

public class UserAggregateRootConfiguration: IEntityTypeConfiguration<UserAggregateRoot>
{
    public void Configure(EntityTypeBuilder<UserAggregateRoot> builder)
    {
        builder.HasNoKey();
    }
}