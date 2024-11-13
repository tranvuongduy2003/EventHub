using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.User.Unfollow;

public record UnfollowCommand(string AccessToken, Guid FollowedUserId) : ICommand;