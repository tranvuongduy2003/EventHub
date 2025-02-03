using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Payment;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Aggregates.PaymentAggregate.Entities;
using EventHub.Domain.Aggregates.PaymentAggregate.ValueObjects;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
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
        Domain.Aggregates.CouponAggregate.Coupon coupon = null;
        if (request.CouponId != null)
        {
            coupon = await _unitOfWork.Coupons.GetByIdAsync((Guid)request.CouponId);
        }

        long totalPrice = request.CheckoutItems?.Sum(x => x.Price * x.Quantity) ?? 0;

        var payment = new Domain.Aggregates.PaymentAggregate.Payment
        {
            AuthorId = request.UserId,
            CustomerEmail = request.CustomerEmail,
            CustomerName = request.CustomerName,
            CustomerPhone = request.CustomerPhone,
            Discount = totalPrice * (coupon?.PercentValue ?? 100) / 100,
            EventId = request.EventId,
            Status = Domain.Shared.Enums.Payment.EPaymentStatus.PENDING,
            TicketQuantity = request.CheckoutItems!.Sum(x => x.Quantity),
            CouponId = request.CouponId,
            TotalPrice = totalPrice,
        };

        await _unitOfWork.Payments.CreateAsync(payment);
        await _unitOfWork.CommitAsync();

        List<PaymentItem> paymentItems = _mapper.Map<List<PaymentItem>>(request.CheckoutItems);
        await _unitOfWork.PaymentItems.CreateListAsync(paymentItems);
        var lineItems = paymentItems
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

        SessionDiscountOptions sessionDiscountOption = null;
        if (coupon != null && coupon.Quantity > 0)
        {
            await _unitOfWork.PaymentCoupons.CreateAsync(new PaymentCoupon { CouponId = coupon.Id, PaymentId = payment.Id });
            sessionDiscountOption = new SessionDiscountOptions
            {
                Coupon = coupon.Code
            };
        }
        else
        {
            throw new BadRequestException("Coupon is out of stock or invalid");
        }

        await _unitOfWork.CommitAsync();

        payment.PaymentItems = paymentItems;

        var options = new SessionCreateOptions
        {
            SuccessUrl = request.SuccessUrl,
            CancelUrl = request.CancelUrl,
            LineItems = lineItems,
            Mode = "payment",
            Discounts = new List<SessionDiscountOptions> { sessionDiscountOption }
        };

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
