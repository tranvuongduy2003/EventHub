using EventHub.Domain.AggregateModels.CategoryAggregate;
using EventHub.Domain.AggregateModels.ConversationAggregate;
using EventHub.Domain.AggregateModels.EmailLoggerAggregate;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.AggregateModels.LabelAggregate;
using EventHub.Domain.AggregateModels.PaymentAggregate;
using EventHub.Domain.AggregateModels.PermissionAggregate;
using EventHub.Domain.AggregateModels.ReviewAggregate;
using EventHub.Domain.AggregateModels.TicketAggregate;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.SeedWork.Interfaces;
using EventHub.Persistence.Outbox;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Persistence.Data;

public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<UserAggregateRoot> UserAggregates { set; get; }
    public DbSet<User> Users { set; get; }
    public DbSet<Role> Roles { set; get; }
    public DbSet<Category> Categories { set; get; }
    public DbSet<Command> Commands { set; get; }
    public DbSet<CommandInFunction> CommandInFunctions { set; get; }
    public DbSet<EmailAttachment> EmailAttachments { set; get; }
    public DbSet<EmailContent> EmailContents { set; get; }
    public DbSet<EmailLogger> EmailLoggers { set; get; }
    public DbSet<Event> Events { set; get; }
    public DbSet<EventSubImage> EventSubImages { set; get; }
    public DbSet<FavouriteEvent> FavouriteEvents { set; get; }
    public DbSet<Function> Functions { set; get; }
    public DbSet<Label> Labels { set; get; }
    public DbSet<LabelInEvent> LabelInEvents { set; get; }
    public DbSet<LabelInUser> LabelInUsers { set; get; }
    public DbSet<Payment> Payments { set; get; }
    public DbSet<PermissionAggregateRoot> PermissionAggregates { set; get; }
    public DbSet<Permission> Permissions { set; get; }
    public DbSet<Review> Reviews { set; get; }
    public DbSet<Ticket> Tickets { set; get; }
    public DbSet<TicketType> TicketTypes { set; get; }
    public DbSet<UserFollower> UserFollowers { set; get; }
    public DbSet<EventCategory> EventCategories { set; get; }
    public DbSet<Reason> Reasons { set; get; }
    public DbSet<Invitation> Invitations { set; get; }
    public DbSet<Conversation> Conversations { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<PaymentItem> PaymentItems { get; set; }
    public DbSet<UserPaymentMethod> UserPaymentMethods { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public DbSet<OutboxMessageConsumer> OutboxMessageConsumers { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var modified = ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Modified or EntityState.Added);

        foreach (var item in modified)
            if (item.Entity is IDateTracking changedOrAddedItem)
            {
                if (item.State == EntityState.Added)
                    changedOrAddedItem.CreatedAt = DateTime.UtcNow;
                else
                    changedOrAddedItem.UpdatedAt = DateTime.UtcNow;
            }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}