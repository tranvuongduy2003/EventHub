using EventHub.Application.SeedWork.DTOs.Payment;
using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Payment.ValidateSession;

public record ValidateSessionCommand(Guid PaymentId) : ICommand<ValidateSessionResponseDto>;
