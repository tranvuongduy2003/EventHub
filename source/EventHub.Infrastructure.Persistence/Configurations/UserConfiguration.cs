using EventHub.Domain.Aggregates.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHub.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasMany(x => x.Payments)
            .WithOne(x => x.Author)
            .HasForeignKey(x => x.AuthorId);
        
        builder
            .HasMany(x => x.Events)
            .WithOne(x => x.Author)
            .HasForeignKey(x => x.AuthorId);
        
        builder
            .HasMany(x => x.Reviews)
            .WithOne(x => x.Author)
            .HasForeignKey(x => x.AuthorId);
        
        builder
            .HasMany(x => x.UserPaymentMethods)
            .WithOne(x => x.Author)
            .HasForeignKey(x => x.AuthorId);
        
        builder
            .HasMany(x => x.UserConversations)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);
        
        builder
            .HasMany(x => x.HostConversations)
            .WithOne(x => x.Host)
            .HasForeignKey(x => x.HostId);
        
        builder
            .HasMany(x => x.Messages)
            .WithOne(x => x.Author)
            .HasForeignKey(x => x.AuthorId);
        
        builder
            .HasMany(x => x.Tickets)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);
    }
}