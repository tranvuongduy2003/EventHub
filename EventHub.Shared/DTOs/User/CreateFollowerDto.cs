namespace EventHub.Shared.DTOs.User;

public class CreateFollowerDto
{
    public string FollowerId { get; set; }

    public string FollowedId { get; set; }
}