using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using EventHub.Domain.AggregateModels.ConversationAggregate;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.AggregateModels.LabelAggregate;
using EventHub.Domain.AggregateModels.PaymentAggregate;
using EventHub.Domain.AggregateModels.ReviewAggregate;
using EventHub.Domain.AggregateModels.TicketAggregate;
using EventHub.Domain.SeedWork.Interfaces;
using EventHub.Shared.Enums.User;
using Microsoft.AspNetCore.Identity;

namespace EventHub.Domain.AggregateModels.UserAggregate;

public class User : IdentityUser<Guid>, IDateTracking, ISoftDeletable
{
    [MaxLength(255)]
    [Column(TypeName = "nvarchar(255)")]
    public string? FullName { get; set; }

    public DateTime? Dob { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EGender? Gender { get; set; }

    [MaxLength(1000)]
    [Column(TypeName = "nvarchar(1000)")]
    public string? Bio { get; set; }

    [MaxLength(255)]
    [Column(TypeName = "nvarchar(255)")]
    public string? AvatarUrl { get; set; }

    [MaxLength(255)]
    [Column(TypeName = "nvarchar(255)")]
    public string? AvatarFileName { get; set; }

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required EUserStatus Status { get; set; } = EUserStatus.ACTIVE;

    [Range(0, double.PositiveInfinity)] 
    public int? NumberOfFollowers { get; set; } = 0;

    [Range(0, double.PositiveInfinity)] 
    public int? NumberOfFolloweds { get; set; } = 0;

    [Range(0, double.PositiveInfinity)] 
    public int? NumberOfFavourites { get; set; } = 0;

    [Range(0, double.PositiveInfinity)] 
    public int? NumberOfCreatedEvents { get; set; } = 0;

    public bool IsDeleted { get; set; } = false;
    
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
}