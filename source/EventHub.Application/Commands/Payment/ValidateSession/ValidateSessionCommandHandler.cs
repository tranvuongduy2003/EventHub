using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Payment;
using EventHub.Application.SeedWork.DTOs.Ticket;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Aggregates.PaymentAggregate.Entities;
using EventHub.Domain.Aggregates.TicketAggregate;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
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

    public ValidateSessionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ValidateSessionResponseDto> Handle(ValidateSessionCommand request, CancellationToken cancellationToken)
    {
        Domain.Aggregates.PaymentAggregate.Payment payment = await _unitOfWork.Payments.GetByIdAsync(request.PaymentId);
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

            var tickets = new List<Ticket>();

            foreach (PaymentItem item in paymentItems)
            {
                for (int i = 0; i < item.Quantity; i++)
                {
                    tickets.Add(new Ticket
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
            }

            await _unitOfWork.Tickets.CreateListAsync(tickets);

            await _unitOfWork.CommitAsync();

            ValidateSessionResponseDto validateSessionResponse = _mapper.Map<ValidateSessionResponseDto>(payment);
            validateSessionResponse.Tickets = _mapper.Map<List<TicketDto>>(tickets);

            // Send email to customer

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
