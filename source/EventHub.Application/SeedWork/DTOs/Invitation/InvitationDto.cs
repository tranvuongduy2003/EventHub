using EventHub.Application.SeedWork.DTOs.Event;
using EventHub.Application.SeedWork.DTOs.User;

namespace EventHub.Application.SeedWork.DTOs.Invitation;

public class InvitationDto
{
    public Guid InviterId { get; set; } = Guid.Empty;

    public Guid InvitedId { get; set; } = Guid.Empty;

    public Guid EventId { get; set; } = Guid.Empty;

    public UserDto Inviter { get; set; }

    public UserDto Invited { get; set; }

    public LeanEventDto Event { get; set; }

    public DateTime CreatedAt { get; set; }
}
