namespace EventHub.Shared.DTOs.Payment;

public class UserPaymentMethodDto
{
    public string Id { get; set; }

    public string UserId { get; set; }

    public string MethodId { get; set; }

    public PaymentMethodDto Method { get; set; }

    public string PaymentAccountNumber { get; set; }

    public string? PaymentAccountQRCode { get; set; }

    public string? CheckoutContent { get; set; } = string.Empty;
}