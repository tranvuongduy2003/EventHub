﻿using EventHub.Domain.Aggregates.CategoryAggregate;
using EventHub.Domain.Aggregates.ConversationAggregate;
using EventHub.Domain.Aggregates.EmailLoggerAggregate;
using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.Aggregates.LabelAggregate;
using EventHub.Domain.Aggregates.PaymentAggregate;
using EventHub.Domain.Aggregates.PermissionAggregate;
using EventHub.Domain.Aggregates.ReviewAggregate;
using EventHub.Domain.Aggregates.TicketAggregate;
using EventHub.Domain.Aggregates.UserAggregate;
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

    #region EmailLoggerAggregate

    public DbSet<EmailLogger> EmailLoggers { set; get; }

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

    #endregion

    #region LabelAggregate

    public DbSet<Label> Labels { set; get; }
    public DbSet<LabelInEvent> LabelInEvents { set; get; }
    public DbSet<LabelInUser> LabelInUsers { set; get; }

    #endregion

    #region PaymentAggregate

    public DbSet<Payment> Payments { set; get; }
    public DbSet<PaymentItem> PaymentItems { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }

    #endregion

    #region PermissionAggregate

    public DbSet<Command> Commands { set; get; }
    public DbSet<CommandInFunction> CommandInFunctions { set; get; }
    public DbSet<Function> Functions { set; get; }
    public DbSet<Permission> Permissions { set; get; }
    public DbSet<PermissionAggregateRoot> PermissionAggregates { set; get; }

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
    public DbSet<UserPaymentMethod> UserPaymentMethods { get; set; }

    #endregion

    #region Outbox

    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public DbSet<OutboxMessageConsumer> OutboxMessageConsumers { get; set; }

    #endregion

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
        
        base.OnModelCreating(builder);
    }
}
