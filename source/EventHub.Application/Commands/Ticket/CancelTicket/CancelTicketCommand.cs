using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Ticket.CancelTicket;

public record CancelTicketCommand(Guid TicketId) : ICommand;
