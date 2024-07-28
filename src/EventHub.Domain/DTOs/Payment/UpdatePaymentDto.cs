namespace EventHub.Domain.DTOs.Payment;

public class UpdatePaymentDto
{
    public string CustomerName { get; set; }

    public string CustomerPhone { get; set; }

    public string CustomerEmail { get; set; }
}