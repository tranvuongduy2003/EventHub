using EventHub.Domain.AggregateModels.PermissionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHub.Persistence.Configurations;

public class CommandConfiguration: IEntityTypeConfiguration<Command>
{
    public void Configure(EntityTypeBuilder<Command> builder)
    {
        builder
            .Property(x => x.Id)
            .HasMaxLength(50)
            .IsUnicode(false);
    }
}