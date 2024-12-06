using Microsoft.AspNetCore.Http;

namespace EventHub.Application.DTOs.Payment;

public class CreatePaymentAccountDto
{
    public string UserId { get; set; }

    public string MethodId { get; set; }

    public string PaymentAccountNumber { get; set; }

    public IFormFile? PaymentAccountQRCode { get; set; }

    public string? CheckoutContent { get; set; } = string.Empty;
}
