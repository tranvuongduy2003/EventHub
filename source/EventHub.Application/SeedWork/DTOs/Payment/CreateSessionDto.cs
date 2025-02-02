namespace EventHub.Application.SeedWork.DTOs.Payment;

public class CreateSessionDto
{
    public string ApprovedUrl { get; set; }

    public string CancelUrl { get; set; }

    public Guid PaymentId { get; set; }
}
