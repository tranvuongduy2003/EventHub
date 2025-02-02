using EventHub.Application.SeedWork.DTOs.Payment;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Aggregates.PaymentAggregate.Entities;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;

namespace EventHub.Application.Commands.Payment.CreateSession;

public class CreateSessionCommandHandler : ICommandHandler<CreateSessionCommand, CreateSessionResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateSessionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateSessionResponseDto> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
    {
        Domain.Aggregates.PaymentAggregate.Payment payment = await _unitOfWork.Payments.GetByIdAsync(request.PaymentId);
        if (payment == null)
        {
            throw new NotFoundException("Pyament does not exist!");
        }

        var options = new SessionCreateOptions
        {
            SuccessUrl = request.ApprovedUrl,
            CancelUrl = request.CancelUrl,
            LineItems = new List<SessionLineItemOptions>(),
            Mode = "payment",
        };

        List<Domain.Aggregates.CouponAggregate.Coupon> coupons = await _unitOfWork.PaymentCoupons
            .FindByCondition(x => x.PaymentId == request.PaymentId)
            .Include(x => x.Coupon)
            .Select(x => x.Coupon)
            .ToListAsync(cancellationToken);

        options.Discounts = coupons
            .Select(x => new SessionDiscountOptions
            {
                Coupon = x.Code
            })
            .ToList();

        List<PaymentItem> paymentItems = await _unitOfWork.PaymentItems
            .FindByCondition(x => x.PaymentId == request.PaymentId)
            .ToListAsync(cancellationToken);

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

        // Create tickets

        return new CreateSessionResponseDto
        {
            SessionId = session.Id,
            SessionUrl = session.Url,
        };
    }
}
