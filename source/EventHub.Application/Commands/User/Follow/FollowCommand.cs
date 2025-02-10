using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.User.Follow;

public record FollowCommand(Guid FollowedUserId) : ICommand<Guid>;
