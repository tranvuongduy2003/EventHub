using EventHub.Shared.Enums.Payment;

namespace EventHub.Shared.DTOs.Payment;

public class UpdatePaymentStatusDto
{
    public EPaymentStatus Status { get; set; }
}