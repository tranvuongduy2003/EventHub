namespace EventHub.Application.SeedWork.DTOs.User;

public class InviteDto
{
    public Guid EventId { get; set; }

    public List<Guid> UserIds { get; set; }
}
