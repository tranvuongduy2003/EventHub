using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Ticket;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Ticket.GetPaginatedTicketsByUserId;

public class GetPaginatedTicketsByUserIdQueryHandler : IQueryHandler<GetPaginatedTicketsByUserIdQuery, Pagination<TicketDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;

    public GetPaginatedTicketsByUserIdQueryHandler(IUnitOfWork unitOfWork, UserManager<Domain.Aggregates.UserAggregate.User> userManager, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Pagination<TicketDto>> Handle(GetPaginatedTicketsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        Domain.Aggregates.UserAggregate.User user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
        {
            throw new NotFoundException("User does not exist!");
        }

        Pagination<Domain.Aggregates.TicketAggregate.Ticket> paginatedTickets = _unitOfWork.Tickets
            .PaginatedFindByCondition(x => x.AuthorId == request.UserId, request.Filter, query => query
                .Include(x => x.Event)
                .Include(x => x.TicketType)
            );

        Pagination<TicketDto> paginatedTicketDtos = _mapper.Map<Pagination<TicketDto>>(paginatedTickets);

        return paginatedTicketDtos;
    }
}
