using EventHub.Domain.Enums.Payment;

namespace EventHub.Domain.DTOs.Payment;

public class UpdatePaymentStatusDto
{
    public EPaymentStatus Status { get; set; }
}