using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.Shared.Enums.Ticket;

namespace EventHub.Application.Commands.Ticket.CancelTicket;

public class CancelTicketCommandHandler : ICommandHandler<CancelTicketCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public CancelTicketCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CancelTicketCommand request, CancellationToken cancellationToken)
    {
        Domain.Aggregates.TicketAggregate.Ticket ticket = await _unitOfWork.Tickets.GetByIdAsync(request.TicketId);
        if (ticket is null)
        {
            throw new NotFoundException("Ticket does not exist!");
        }

        ticket.Status = ETicketStatus.INACTIVE;

        await _unitOfWork.Tickets.Update(ticket);
        await _unitOfWork.CommitAsync();
    }
}
