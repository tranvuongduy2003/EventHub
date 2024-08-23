using EventHub.Domain.AggregateModels.EventAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHub.Persistence.Configurations;

public class EmailContentConfiguration: IEntityTypeConfiguration<EmailContent>
{
    public void Configure(EntityTypeBuilder<EmailContent> builder)
    {
        builder
            .HasMany(x => x.EmailAttachments)
            .WithOne(x => x.EmailContent)
            .HasForeignKey(x => x.EmailContentId);
        builder
            .HasMany(x => x.EmailLoggers)
            .WithOne(x => x.EmailContent)
            .HasForeignKey(x => x.EmailContentId);
    }
}

