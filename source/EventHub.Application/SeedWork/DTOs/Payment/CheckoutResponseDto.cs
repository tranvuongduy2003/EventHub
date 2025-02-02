namespace EventHub.Application.SeedWork.DTOs.Payment;

public class CheckoutResponseDto
{
    public string SessionUrl { get; set; }

    public string SessionId { get; set; }

    public Guid PaymentId { get; set; }
}
