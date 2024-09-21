using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.SeedWork.AggregateRoot;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.AggregateModels.ConversationAggregate;

[Table("Conversations")]
public class Conversation : AggregateRoot
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required] public required Guid EventId { get; set; } = Guid.Empty;

    [Required] public required Guid HostId { get; set; } = Guid.Empty;

    [Required] public required Guid UserId { get; set; } = Guid.Empty;

    public Guid? LastMessageId { get; set; }

    [ForeignKey("EventId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Event Event { get; set; } = null!;

    [ForeignKey("HostId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual User Host { get; set; } = null!;

    [ForeignKey("AuthorId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual User User { get; set; } = null!;

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}