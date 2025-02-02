using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Payment;
using EventHub.Domain.Aggregates.PaymentAggregate.Entities;
using EventHub.Domain.Aggregates.PaymentAggregate.ValueObjects;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;

namespace EventHub.Application.Commands.Payment.Checkout;

public class CheckoutCommandHandler : ICommandHandler<CheckoutCommand, CheckoutResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CheckoutCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CheckoutResponseDto> Handle(CheckoutCommand request, CancellationToken cancellationToken)
    {
        Domain.Aggregates.CouponAggregate.Coupon coupon = await _unitOfWork.Coupons.GetByIdAsync(request.CouponId);

        long totalPrice = request.CheckoutItems?.Sum(x => x.Price * x.Quantity) ?? 0;

        var payment = new Domain.Aggregates.PaymentAggregate.Payment
        {
            AuthorId = request.UserId,
            CustomerEmail = request.CustomerEmail,
            CustomerName = request.CustomerName,
            CustomerPhone = request.CustomerPhone,
            Discount = totalPrice * coupon.PercentValue / 100,
            EventId = request.EventId,
            Status = Domain.Shared.Enums.Payment.EPaymentStatus.PENDING,
            TicketQuantity = request.CheckoutItems!.Sum(x => x.Quantity),
            TotalPrice = totalPrice,
        };

        await _unitOfWork.Payments.CreateAsync(payment);
        await _unitOfWork.CommitAsync();

        List<PaymentItem> paymentItems = _mapper.Map<List<PaymentItem>>(request.CheckoutItems);
        await _unitOfWork.PaymentItems.CreateListAsync(paymentItems);
        await _unitOfWork.PaymentCoupons.CreateAsync(new PaymentCoupon { CouponId = request.CouponId, PaymentId = payment.Id });

        await _unitOfWork.CommitAsync();

        payment.PaymentItems = paymentItems;

        var options = new SessionCreateOptions
        {
            SuccessUrl = request.SuccessUrl,
            CancelUrl = request.CancelUrl,
            LineItems = new List<SessionLineItemOptions>(),
            Mode = "payment",
        };

        List<Domain.Aggregates.CouponAggregate.Coupon> coupons = await _unitOfWork.PaymentCoupons
            .FindByCondition(x => x.PaymentId == payment.Id)
            .Include(x => x.Coupon)
            .Select(x => x.Coupon)
            .ToListAsync(cancellationToken);

        options.Discounts = coupons
            .Select(x => new SessionDiscountOptions
            {
                Coupon = x.Code
            })
            .ToList();

        options.LineItems = paymentItems
            .Select(x => new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = x.TotalPrice,
                    Currency = "vnd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = $"{x.Name}",
                    },
                },
                Quantity = x.Quantity,
            })
            .ToList();

        var service = new SessionService();
        Session session = await service.CreateAsync(options, cancellationToken: cancellationToken);
        payment.SessionId = session.Id;

        await _unitOfWork.Payments.Update(payment);

        return new CheckoutResponseDto
        {
            SessionId = session.Id,
            SessionUrl = session.Url,
            PaymentId = payment.Id,
        };
    }
}
