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

    public DbSet<UserAggregate> UserAggregates { set; get; }
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

        builder.Entity<User>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        builder.Entity<Role>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        builder.Entity<Label>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        builder.Entity<Command>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        builder.Entity<Function>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        builder.Entity<Category>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        builder.Entity<Event>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        builder.Entity<Review>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        builder.Entity<EmailLogger>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        builder.Entity<EventSubImage>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        builder.Entity<TicketType>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        builder.Entity<EmailContent>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        builder.Entity<Payment>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        builder.Entity<Ticket>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        builder.Entity<Reason>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        builder.Entity<Conversation>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        builder.Entity<Message>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        builder.Entity<PaymentItem>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        builder.Entity<UserPaymentMethod>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        builder.Entity<PaymentMethod>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);


        #region Many-to-many

        builder.Entity<Invitation>()
            .HasOne(x => x.Inviter)
            .WithMany(x => x.Inviters)
            .HasForeignKey(x => x.InviterId);
        builder.Entity<Invitation>()
            .HasOne(x => x.Invited)
            .WithMany(x => x.Inviteds)
            .HasForeignKey(x => x.InvitedId);

        builder.Entity<UserFollower>()
            .HasOne(x => x.Followed)
            .WithMany(x => x.Followeds)
            .HasForeignKey(x => x.FollowedId);
        builder.Entity<UserFollower>()
            .HasOne(x => x.Follower)
            .WithMany(x => x.Followers)
            .HasForeignKey(x => x.FollowerId);

        builder.Entity<LabelInUser>()
            .HasOne(x => x.User)
            .WithMany(x => x.LabelInUsers)
            .HasForeignKey(x => x.UserId);
        builder.Entity<LabelInUser>()
            .HasOne(x => x.Label)
            .WithMany(x => x.LabelInUsers)
            .HasForeignKey(x => x.LabelId);

        builder.Entity<FavouriteEvent>()
            .HasOne(x => x.Event)
            .WithMany(x => x.FavouriteEvents)
            .HasForeignKey(x => x.EventId);
        builder.Entity<FavouriteEvent>()
            .HasOne(x => x.User)
            .WithMany(x => x.FavouriteEvents)
            .HasForeignKey(x => x.UserId);

        builder.Entity<CommandInFunction>()
            .HasOne(x => x.Function)
            .WithMany(x => x.CommandInFunctions)
            .HasForeignKey(x => x.FunctionId);
        builder.Entity<CommandInFunction>()
            .HasOne(x => x.Command)
            .WithMany(x => x.CommandInFunctions)
            .HasForeignKey(x => x.CommandId);

        builder.Entity<Permission>()
            .HasOne(x => x.Function)
            .WithMany(x => x.Permissions)
            .HasForeignKey(x => x.FunctionId);
        builder.Entity<Permission>()
            .HasOne(x => x.Role)
            .WithMany(x => x.Permissions)
            .HasForeignKey(x => x.RoleId);
        builder.Entity<Permission>()
            .HasOne(x => x.Command)
            .WithMany(x => x.Permissions)
            .HasForeignKey(x => x.CommandId);

        builder.Entity<EventCategory>()
            .HasOne(x => x.Event)
            .WithMany(x => x.EventCategories)
            .HasForeignKey(x => x.EventId);
        builder.Entity<EventCategory>()
            .HasOne(x => x.Category)
            .WithMany(x => x.EventCategories)
            .HasForeignKey(x => x.CategoryId);

        builder.Entity<LabelInEvent>()
            .HasOne(x => x.Event)
            .WithMany(x => x.LabelInEvents)
            .HasForeignKey(x => x.EventId);
        builder.Entity<LabelInEvent>()
            .HasOne(x => x.Label)
            .WithMany(x => x.LabelInEvents)
            .HasForeignKey(x => x.LabelId);

        #endregion

        #region One-to-many

        builder.Entity<User>()
            .HasMany(x => x.Payments)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);
        builder.Entity<User>()
            .HasMany(x => x.Events)
            .WithOne(x => x.Author)
            .HasForeignKey(x => x.AuthorId);
        builder.Entity<User>()
            .HasMany(x => x.Reviews)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);
        builder.Entity<User>()
            .HasMany(x => x.UserPaymentMethods)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);
        builder.Entity<User>()
            .HasMany(x => x.UserConversations)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);
        builder.Entity<User>()
            .HasMany(x => x.HostConversations)
            .WithOne(x => x.Host)
            .HasForeignKey(x => x.HostId);
        builder.Entity<User>()
            .HasMany(x => x.Messages)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);
        builder.Entity<User>()
            .HasMany(x => x.PaymentItems)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);
        builder.Entity<User>()
            .HasMany(x => x.Tickets)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);

        builder.Entity<Event>()
            .HasMany(x => x.EventSubImages)
            .WithOne(x => x.Event)
            .HasForeignKey(x => x.EventId);
        builder.Entity<Event>()
            .HasMany(x => x.TicketTypes)
            .WithOne(x => x.Event)
            .HasForeignKey(x => x.EventId);
        builder.Entity<Event>()
            .HasMany(x => x.Invitations)
            .WithOne(x => x.Event)
            .HasForeignKey(x => x.EventId);
        builder.Entity<Event>()
            .HasMany(x => x.Payments)
            .WithOne(x => x.Event)
            .HasForeignKey(x => x.EventId);
        builder.Entity<Event>()
            .HasMany(x => x.Reviews)
            .WithOne(x => x.Event)
            .HasForeignKey(x => x.EventId);
        builder.Entity<Event>()
            .HasMany(x => x.Tickets)
            .WithOne(x => x.Event)
            .HasForeignKey(x => x.EventId);
        builder.Entity<Event>()
            .HasMany(x => x.Conversations)
            .WithOne(x => x.Event)
            .HasForeignKey(x => x.EventId);
        builder.Entity<Event>()
            .HasMany(x => x.Messages)
            .WithOne(x => x.Event)
            .HasForeignKey(x => x.EventId);
        builder.Entity<Event>()
            .HasMany(x => x.PaymentItems)
            .WithOne(x => x.Event)
            .HasForeignKey(x => x.EventId);
        builder.Entity<Event>()
            .HasMany(x => x.Reasons)
            .WithOne(x => x.Event)
            .HasForeignKey(x => x.EventId);

        builder.Entity<Conversation>()
            .HasMany(x => x.Messages)
            .WithOne(x => x.Conversation)
            .HasForeignKey(x => x.ConversationId);

        builder.Entity<EmailContent>()
            .HasMany(x => x.EmailAttachments)
            .WithOne(x => x.EmailContent)
            .HasForeignKey(x => x.EmailContentId);
        builder.Entity<EmailContent>()
            .HasMany(x => x.EmailLoggers)
            .WithOne(x => x.EmailContent)
            .HasForeignKey(x => x.EmailContentId);

        builder.Entity<Payment>()
            .HasMany(x => x.Tickets)
            .WithOne(x => x.Payment)
            .HasForeignKey(x => x.PaymentId);
        builder.Entity<Payment>()
            .HasMany(x => x.PaymentItems)
            .WithOne(x => x.Payment)
            .HasForeignKey(x => x.PaymentId);

        builder.Entity<PaymentMethod>()
            .HasMany(x => x.UserPaymentMethods)
            .WithOne(x => x.Method)
            .HasForeignKey(x => x.MethodId);

        builder.Entity<UserPaymentMethod>()
            .HasMany(x => x.Payments)
            .WithOne(x => x.UserPaymentMethod)
            .HasForeignKey(x => x.UserPaymentMethodId);

        builder.Entity<TicketType>()
            .HasMany(x => x.PaymentItems)
            .WithOne(x => x.TicketType)
            .HasForeignKey(x => x.TicketTypeId);
        builder.Entity<TicketType>()
            .HasMany(x => x.Tickets)
            .WithOne(x => x.TicketType)
            .HasForeignKey(x => x.TicketTypeId);

        #endregion
    }
}