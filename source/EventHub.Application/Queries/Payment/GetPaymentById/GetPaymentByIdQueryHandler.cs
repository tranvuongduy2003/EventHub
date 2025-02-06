using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Payment;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Payment.GetPaymentById;

public class GetPaymentByIdQueryHandler : IQueryHandler<GetPaymentByIdQuery, PaymentDto>
{

    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetPaymentByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaymentDto> Handle(GetPaymentByIdQuery request,
        CancellationToken cancellationToken)
    {
        Domain.Aggregates.PaymentAggregate.Payment ticket = await _unitOfWork.Payments
            .FindByCondition(x => x.Id == request.PaymentId)
            .Include(x => x.Event)
                .ThenInclude(x => x.Author)
            .Include(x => x.Coupon)
            .Include(x => x.PaymentItems)
            .Include(x => x.Author)
            .FirstOrDefaultAsync(cancellationToken);

        if (ticket == null)
        {
            throw new NotFoundException("Payment does not exist!");
        }

        return _mapper.Map<PaymentDto>(ticket);
    }
}
