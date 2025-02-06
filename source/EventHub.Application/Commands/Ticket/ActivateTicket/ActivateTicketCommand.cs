using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Ticket.ActivateTicket;

public record ActivateTicketCommand(Guid TicketId) : ICommand;
