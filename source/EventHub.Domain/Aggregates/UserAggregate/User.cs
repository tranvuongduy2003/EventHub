﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using EventHub.Domain.Aggregates.ConversationAggregate;
using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.Aggregates.LabelAggregate;
using EventHub.Domain.Aggregates.PaymentAggregate;
using EventHub.Domain.Aggregates.ReviewAggregate;
using EventHub.Domain.Aggregates.TicketAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.AggregateRoot;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Domain.SeedWork.Entities;
using EventHub.Domain.Shared.Enums.User;
using Microsoft.AspNetCore.Identity;

namespace EventHub.Domain.Aggregates.UserAggregate;

public class User : IdentityUser<Guid>, IAggregateRoot, IDateTracking, ISoftDeletable
{
    private readonly List<IDomainEvent> _domainEvents = new();

    [MaxLength(255)]
    [Column(TypeName = "nvarchar(255)")]
    public string? FullName { get; set; }

    public DateTime? Dob { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EGender? Gender { get; set; }

    [MaxLength(1000)]
    [Column(TypeName = "nvarchar(1000)")]
    public string? Bio { get; set; }

    [Column(TypeName = "nvarchar(max)")] public string? AvatarUrl { get; set; }

    [MaxLength(255)]
    [Column(TypeName = "nvarchar(255)")]
    public string? AvatarFileName { get; set; }

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EUserStatus Status { get; set; } = EUserStatus.ACTIVE;

    [Range(0, double.PositiveInfinity)] public int? NumberOfFollowers { get; set; } = 0;

    [Range(0, double.PositiveInfinity)] public int? NumberOfFolloweds { get; set; } = 0;

    [Range(0, double.PositiveInfinity)] public int? NumberOfFavourites { get; set; } = 0;

    [Range(0, double.PositiveInfinity)] public int? NumberOfCreatedEvents { get; set; } = 0;

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<LabelInUser> LabelInUsers { get; set; } = new List<LabelInUser>();

    public virtual ICollection<UserFollower> Followers { get; set; } = new List<UserFollower>();
    public virtual ICollection<UserFollower> Followeds { get; set; } = new List<UserFollower>();

    public virtual ICollection<Invitation> Inviteds { get; set; } = new List<Invitation>();
    public virtual ICollection<Invitation> Inviters { get; set; } = new List<Invitation>();

    public virtual ICollection<FavouriteEvent> FavouriteEvents { get; set; } = new List<FavouriteEvent>();

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<PaymentItem> PaymentItems { get; set; } = new List<PaymentItem>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Conversation> UserConversations { get; set; } = new List<Conversation>();
    public virtual ICollection<Conversation> HostConversations { get; set; } = new List<Conversation>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual ICollection<UserPaymentMethod> UserPaymentMethods { get; set; } = new List<UserPaymentMethod>();

    public virtual ICollection<Role>? Roles { get; set; } = null!;

    public void ChangeUserPassword(Guid userId, string oldPassword, string newPassword)
    {
        RaiseDomainEvent(new ChangeUserPasswordDomainEvent(Guid.NewGuid(), userId, oldPassword, newPassword));
    }

    public void FollowUser(Guid followerId, Guid followedUserId)
    {
        RaiseDomainEvent(new FollowUserDomainEvent(Guid.NewGuid(), followerId, followedUserId));
    }

    public void UnfollowUser(Guid followerId, Guid followedUserId)
    {
        RaiseDomainEvent(new UnfollowUserDomainEvent(Guid.NewGuid(), followerId, followedUserId));
    }

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();

    public void ClearDomainEvents() => _domainEvents.Clear();

    public void RaiseDomainEvent(IDomainEvent domainEvent) =>
        _domainEvents.Add(domainEvent);
}
