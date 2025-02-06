using EventHub.Domain.Aggregates.ConversationAggregate;
using EventHub.Domain.Aggregates.ConversationAggregate.Entities;
using EventHub.Domain.Aggregates.CouponAggregate;
using EventHub.Domain.Aggregates.CouponAggregate.ValueObjects;
using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.Aggregates.EventAggregate.Entities;
using EventHub.Domain.Aggregates.EventAggregate.ValueObjects;
using EventHub.Domain.Aggregates.NotificationAggregate;
using EventHub.Domain.Aggregates.PaymentAggregate;
using EventHub.Domain.Aggregates.PaymentAggregate.Entities;
using EventHub.Domain.Aggregates.TicketAggregate;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.Aggregates.UserAggregate.Entities;
using EventHub.Domain.Aggregates.UserAggregate.ValueObjects;
using EventHub.Infrastructure.Persistence.Outbox;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Infrastructure.Persistence.Data;

public class ApplicationDbContext(DbContextOptions options) : IdentityDbContext<User, Role, Guid>(options)
{
    #region CategoryAggregate

    public DbSet<Category> Categories { set; get; }

    #endregion

    #region ConversationAggregate

    public DbSet<Conversation> Conversations { get; set; }
    public DbSet<Message> Messages { get; set; }

    #endregion

    #region EventAggregate

    public DbSet<EmailAttachment> EmailAttachments { set; get; }
    public DbSet<EmailContent> EmailContents { set; get; }
    public DbSet<Event> Events { set; get; }
    public DbSet<EventCategory> EventCategories { set; get; }
    public DbSet<EventSubImage> EventSubImages { set; get; }
    public DbSet<FavouriteEvent> FavouriteEvents { set; get; }
    public DbSet<Reason> Reasons { set; get; }
    public DbSet<TicketType> TicketTypes { set; get; }
    public DbSet<Expense> Expenses { set; get; }
    public DbSet<SubExpense> SubExpenses { set; get; }

    #endregion

    #region PaymentAggregate

    public DbSet<Payment> Payments { set; get; }
    public DbSet<PaymentItem> PaymentItems { get; set; }

    #endregion

    #region PermissionAggregate

    public DbSet<Command> Commands { set; get; }
    public DbSet<CommandInFunction> CommandInFunctions { set; get; }
    public DbSet<Function> Functions { set; get; }
    public DbSet<Permission> Permissions { set; get; }

    #endregion

    #region ReviewAggregate

    public DbSet<Review> Reviews { set; get; }

    #endregion

    #region TicketAggregate

    public DbSet<Ticket> Tickets { set; get; }

    #endregion

    #region UserAggregate

    public DbSet<Invitation> Invitations { set; get; }
    public override DbSet<Role> Roles { set; get; }
    public override DbSet<User> Users { set; get; }
    public DbSet<UserFollower> UserFollowers { set; get; }
    #endregion

    #region Outbox

    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public DbSet<OutboxMessageConsumer> OutboxMessageConsumers { get; set; }

    #endregion

    #region CouponAggregate

    public DbSet<Coupon> Coupons { set; get; }
    public DbSet<EventCoupon> EventCoupons { set; get; }
    #endregion

    #region NotificationAggregate

    public DbSet<Notification> Notifications { set; get; }
    #endregion

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);

        base.OnModelCreating(builder);
    }
}
