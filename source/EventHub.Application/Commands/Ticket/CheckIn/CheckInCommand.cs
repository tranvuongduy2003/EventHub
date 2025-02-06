using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Ticket.CheckIn;

public record CheckInCommand(Guid TicketId) : ICommand;
