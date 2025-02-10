using AutoMapper;
using EventHub.Application.SeedWork.Abstractions;
using EventHub.Application.SeedWork.DTOs.Notification;
using EventHub.Application.SeedWork.DTOs.Payment;
using EventHub.Application.SeedWork.DTOs.Ticket;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Aggregates.EventAggregate.Entities;
using EventHub.Domain.Aggregates.PaymentAggregate.Entities;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.Shared.Enums.Notification;
using EventHub.Domain.Shared.Enums.Payment;
using EventHub.Domain.Shared.Helpers;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;

namespace EventHub.Application.Commands.Payment.ValidateSession;

public class ValidateSessionCommandHandler : ICommandHandler<ValidateSessionCommand, ValidateSessionResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly INotificationService _notificationService;

    public ValidateSessionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _notificationService = notificationService;
    }

    public async Task<ValidateSessionResponseDto> Handle(ValidateSessionCommand request, CancellationToken cancellationToken)
    {
        Domain.Aggregates.PaymentAggregate.Payment payment = await _unitOfWork.Payments
            .FindByCondition(x => x.Id == request.PaymentId)
            .Include(x => x.Event)
            .FirstOrDefaultAsync(cancellationToken);

        if (payment == null)
        {
            throw new NotFoundException("Payment does not exist!");
        }

        var service = new SessionService();
        Session session = await service.GetAsync(payment.SessionId, cancellationToken: cancellationToken);

        var paymentIntentService = new PaymentIntentService();
        PaymentIntent paymentIntent = await paymentIntentService.GetAsync(session.PaymentIntentId, cancellationToken: cancellationToken);

        if (paymentIntent.Status == "succeeded")
        {
            payment.PaymentIntentId = paymentIntent.Id;
            payment.Status = EPaymentStatus.SUCCESS;

            await _unitOfWork.Payments.Update(payment);

            List<PaymentItem> paymentItems = await _unitOfWork.PaymentItems
                .FindByCondition(x => x.PaymentId == payment.Id)
                .ToListAsync(cancellationToken);

            var tickets = new List<Domain.Aggregates.TicketAggregate.Ticket>();
            var ticketTypes = new List<TicketType>();

            foreach (PaymentItem item in paymentItems)
            {
                for (int i = 0; i < item.Quantity; i++)
                {
                    tickets.Add(new Domain.Aggregates.TicketAggregate.Ticket
                    {
                        CustomerEmail = payment.CustomerEmail,
                        CustomerName = payment.CustomerName,
                        CustomerPhone = payment.CustomerPhone,
                        EventId = item.EventId,
                        PaymentId = payment.Id,
                        TicketTypeId = item.TicketTypeId,
                        AuthorId = payment.AuthorId,
                        Status = Domain.Shared.Enums.Ticket.ETicketStatus.ACTIVE,
                        TicketNo = TicketCodeGenerator.GenerateTicketCode()
                    });
                }
                TicketType ticketType = await _unitOfWork.TicketTypes.GetByIdAsync(item.TicketTypeId);
                ticketType.NumberOfSoldTickets += item.Quantity;
                ticketTypes.Add(ticketType);
            }
            await _unitOfWork.Tickets.CreateListAsync(tickets);

            if (payment.CouponId != null)
            {
                Domain.Aggregates.CouponAggregate.Coupon coupon = await _unitOfWork.Coupons.GetByIdAsync((Guid)payment.CouponId);

                coupon.Quantity--;

                await _unitOfWork.Coupons.Update(coupon);
            }

            Domain.Aggregates.EventAggregate.Event @event = await _unitOfWork.Events.GetByIdAsync(payment.EventId);
            @event.NumberOfSoldTickets += tickets.Count;
            await _unitOfWork.Events.Update(@event);

            foreach (TicketType ticketType in ticketTypes)
            {
                await _unitOfWork.TicketTypes.Update(ticketType);
            }

            await _unitOfWork.CommitAsync();

            ValidateSessionResponseDto validateSessionResponse = _mapper.Map<ValidateSessionResponseDto>(payment);
            validateSessionResponse.Tickets = _mapper.Map<List<TicketDto>>(tickets);

            // Send email to customer

            var notification = new SendNotificationDto
            {
                Title = "New Ticket Purchase",
                Message = $"A new ticket has been purchased for your event '{@event.Name}' by '{payment.CustomerName}",
                Type = ENotificationType.FOLLOWING,
            };
            await _notificationService.SendNotification(@event.AuthorId.ToString(), notification);

            return validateSessionResponse;
        }
        else
        {
            payment.PaymentIntentId = paymentIntent.Id;
            payment.Status = EPaymentStatus.FAILED;

            await _unitOfWork.Payments.Update(payment);
            await _unitOfWork.CommitAsync();

            throw new BadRequestException("Failed to complete transaction!");
        }
    }
}
