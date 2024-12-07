using EventHub.Domain.Shared.Enums.Payment;

namespace EventHub.Application.SeedWork.DTOs.Payment;

public class UpdatePaymentStatusDto
{
    public EPaymentStatus Status { get; set; }
}
