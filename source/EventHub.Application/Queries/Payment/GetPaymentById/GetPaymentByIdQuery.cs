using EventHub.Application.SeedWork.DTOs.Payment;
using EventHub.Domain.SeedWork.Query;

namespace EventHub.Application.Queries.Payment.GetPaymentById;

public record GetPaymentByIdQuery(Guid PaymentId) : IQuery<PaymentDto>;
