using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Ticket;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Ticket.GetPaginatedTicketsByEventId;

public class GetPaginatedTicketsByEventIdQueryHandler : IQueryHandler<GetPaginatedTicketsByEventIdQuery, Pagination<TicketDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetPaginatedTicketsByEventIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Pagination<TicketDto>> Handle(GetPaginatedTicketsByEventIdQuery request,
        CancellationToken cancellationToken)
    {
        bool isEventExisted = await _unitOfWork.Events.ExistAsync(x => x.Id == request.EventId);
        if (!isEventExisted)
        {
            throw new NotFoundException("Event does not exist!");
        }

        Pagination<Domain.Aggregates.TicketAggregate.Ticket> paginatedTickets = _unitOfWork.Tickets
            .PaginatedFindByCondition(x => x.EventId == request.EventId, request.Filter, query => query
                .Include(x => x.Event)
                .Include(x => x.TicketType)
            );

        Pagination<TicketDto> paginatedTicketDtos = _mapper.Map<Pagination<TicketDto>>(paginatedTickets);

        return paginatedTicketDtos;
    }
}
