namespace EventHub.Application.SeedWork.DTOs.Payment;

public class PaymentAccountDto
{
    public string Id { get; set; }

    public string UserId { get; set; }

    public string MethodId { get; set; }

    public string MethodName { get; set; }

    public string MethodLogo { get; set; }

    public string PaymentAccountNumber { get; set; }

    public string? PaymentAccountQRCode { get; set; }

    public string? CheckoutContent { get; set; } = string.Empty;
}
