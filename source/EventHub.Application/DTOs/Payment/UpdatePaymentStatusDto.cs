using EventHub.Domain.Shared.Enums.Payment;

namespace EventHub.Application.DTOs.Payment;

public class UpdatePaymentStatusDto
{
    public EPaymentStatus Status { get; set; }
}
