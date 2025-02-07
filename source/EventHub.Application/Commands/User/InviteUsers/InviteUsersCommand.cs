using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.User.InviteUsers;

public record InviteUsersCommand(Guid EventId, List<Guid> UserIds) : ICommand;
