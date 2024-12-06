using EventHub.Domain.Aggregates.PermissionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHub.Infrastructure.Persistence.Configurations;

public class FunctionConfiguration: IEntityTypeConfiguration<Function>
{
    public void Configure(EntityTypeBuilder<Function> builder)
    {
        builder
            .Property(x => x.Id)
            .HasMaxLength(50)
            .IsUnicode(false);
    }
}