using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Payment;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Payment.GetPaginatedPayments;

public class GetPaginatedPaymentsQueryHandler : IQueryHandler<GetPaginatedPaymentsQuery, Pagination<PaymentDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetPaginatedPaymentsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public Task<Pagination<PaymentDto>> Handle(GetPaginatedPaymentsQuery request,
        CancellationToken cancellationToken)
    {
        Pagination<Domain.Aggregates.PaymentAggregate.Payment> paginatedPayments = _unitOfWork.Payments
            .PaginatedFind(request.Filter, query => query
                .Include(x => x.Event)
                    .ThenInclude(x => x.Author)
                .Include(x => x.Coupon)
                .Include(x => x.PaymentItems)
                .Include(x => x.Author)
            );

        Pagination<PaymentDto> paginatedPaymentDtos = _mapper.Map<Pagination<PaymentDto>>(paginatedPayments);

        return Task.FromResult(paginatedPaymentDtos);
    }
}
