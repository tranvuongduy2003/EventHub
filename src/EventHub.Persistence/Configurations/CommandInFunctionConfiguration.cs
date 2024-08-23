using EventHub.Domain.AggregateModels.PermissionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHub.Persistence.Configurations;

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