using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Ticket;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Ticket.GetTicketById;

public class GetTicketByIdQueryHandler : IQueryHandler<GetTicketByIdQuery, TicketDto>
{

    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetTicketByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TicketDto> Handle(GetTicketByIdQuery request,
        CancellationToken cancellationToken)
    {
        Domain.Aggregates.TicketAggregate.Ticket ticket = await _unitOfWork.Tickets
            .FindByCondition(x => x.Id == request.TicketId)
            .Include(x => x.Event)
            .Include(x => x.TicketType)
            .FirstOrDefaultAsync(cancellationToken);

        if (ticket == null)
        {
            throw new NotFoundException("Ticket does not exist!");
        }

        return _mapper.Map<TicketDto>(ticket);
    }
}
