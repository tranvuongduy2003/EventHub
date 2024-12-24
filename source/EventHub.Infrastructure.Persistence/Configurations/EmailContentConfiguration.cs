using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.Aggregates.EventAggregate.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHub.Infrastructure.Persistence.Configurations;

public class EmailContentConfiguration: IEntityTypeConfiguration<EmailContent>
{
    public void Configure(EntityTypeBuilder<EmailContent> builder)
    {
        builder
            .HasMany(x => x.EmailAttachments)
            .WithOne(x => x.EmailContent)
            .HasForeignKey(x => x.EmailContentId);
    }
}

