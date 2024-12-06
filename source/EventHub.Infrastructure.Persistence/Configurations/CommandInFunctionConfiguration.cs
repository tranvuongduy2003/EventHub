using EventHub.Domain.Aggregates.PermissionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHub.Infrastructure.Persistence.Configurations;

public class CommandInFunctionConfiguration: IEntityTypeConfiguration<CommandInFunction>
{
    public void Configure(EntityTypeBuilder<CommandInFunction> builder)
    {
        builder
            .HasOne(x => x.Function)
            .WithMany(x => x.CommandInFunctions)
            .HasForeignKey(x => x.FunctionId);
        
        builder
            .HasOne(x => x.Command)
            .WithMany(x => x.CommandInFunctions)
            .HasForeignKey(x => x.CommandId);
    }
}