using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.User.Follow;

public record FollowCommand(string AccessToken, Guid FollowedUserId) : ICommand;