namespace EventHub.Domain.DTOs.Payment;

public class CreatePaymentSessionDto
{
    public string? StripeSessionUrl { get; set; }

    public string? StripeSessionId { get; set; }

    public string ApprovedUrl { get; set; }

    public string CancelUrl { get; set; }

    public int PaymentId { get; set; }
}