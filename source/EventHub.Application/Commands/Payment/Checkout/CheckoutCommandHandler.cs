using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Payment;
using EventHub.Domain.Aggregates.PaymentAggregate.Entities;
using EventHub.Domain.Aggregates.PaymentAggregate.ValueObjects;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Application.Commands.Payment.Checkout;

public class CheckoutCommandHandler : ICommandHandler<CheckoutCommand, PaymentDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CheckoutCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaymentDto> Handle(CheckoutCommand request, CancellationToken cancellationToken)
    {
        var payment = new Domain.Aggregates.PaymentAggregate.Payment
        {
            AuthorId = request.UserId,
            CustomerEmail = request.CustomerEmail,
            CustomerName = request.CustomerName,
            CustomerPhone = request.CustomerPhone,
            Discount = request.Discount,
            EventId = request.EventId,
            Status = Domain.Shared.Enums.Payment.EPaymentStatus.PENDING,
            TicketQuantity = request.CheckoutItems.Sum(x => x.Quantity),
            TotalPrice = request.CheckoutItems.Sum(x => x.TotalPrice),
        };

        await _unitOfWork.Payments.CreateAsync(payment);
        await _unitOfWork.CommitAsync();

        List<PaymentItem> paymentItems = _mapper.Map<List<PaymentItem>>(request.CheckoutItems);
        await _unitOfWork.PaymentItems.CreateListAsync(paymentItems);

        var paymentCoupons = request.CouponIds
            .Select(x => new PaymentCoupon { CouponId = x, PaymentId = payment.Id })
            .ToList();
        await _unitOfWork.PaymentCoupons.CreateListAsync(paymentCoupons);

        await _unitOfWork.CommitAsync();

        payment.PaymentItems = paymentItems;

        return _mapper.Map<PaymentDto>(payment);
    }
}
