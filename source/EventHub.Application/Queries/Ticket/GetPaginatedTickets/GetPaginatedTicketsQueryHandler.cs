using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Ticket;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Ticket.GetPaginatedTickets;

public class GetPaginatedTicketsQueryHandler : IQueryHandler<GetPaginatedTicketsQuery, Pagination<TicketDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetPaginatedTicketsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public Task<Pagination<TicketDto>> Handle(GetPaginatedTicketsQuery request,
        CancellationToken cancellationToken)
    {
        Pagination<Domain.Aggregates.TicketAggregate.Ticket> paginatedTickets = _unitOfWork.Tickets
            .PaginatedFind(request.Filter, query => query
                .Include(x => x.Event)
                .Include(x => x.TicketType)
            );

        Pagination<TicketDto> paginatedTicketDtos = _mapper.Map<Pagination<TicketDto>>(paginatedTickets);

        return Task.FromResult(paginatedTicketDtos);
    }
}
