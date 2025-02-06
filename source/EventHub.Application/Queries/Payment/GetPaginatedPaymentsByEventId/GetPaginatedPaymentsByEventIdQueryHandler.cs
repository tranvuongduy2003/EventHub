using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Payment;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Payment.GetPaginatedPaymentsByEventId;

public class GetPaginatedPaymentsByEventIdQueryHandler : IQueryHandler<GetPaginatedPaymentsByEventIdQuery, Pagination<PaymentDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetPaginatedPaymentsByEventIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Pagination<PaymentDto>> Handle(GetPaginatedPaymentsByEventIdQuery request,
        CancellationToken cancellationToken)
    {
        bool isEventExisted = await _unitOfWork.Events.ExistAsync(x => x.Id == request.EventId);
        if (!isEventExisted)
        {
            throw new NotFoundException("Event does not exist!");
        }

        Pagination<Domain.Aggregates.PaymentAggregate.Payment> paginatedPayments = _unitOfWork.Payments
            .PaginatedFindByCondition(x => x.EventId == request.EventId, request.Filter, query => query
                .Include(x => x.Event)
                    .ThenInclude(x => x.Author)
                .Include(x => x.Coupon)
                .Include(x => x.PaymentItems)
                .Include(x => x.Author)
            );

        Pagination<PaymentDto> paginatedPaymentDtos = _mapper.Map<Pagination<PaymentDto>>(paginatedPayments);

        return paginatedPaymentDtos;
    }
}
