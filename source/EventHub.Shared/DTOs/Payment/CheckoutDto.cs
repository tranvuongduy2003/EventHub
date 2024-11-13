namespace EventHub.Shared.DTOs.Payment;

public class CheckoutDto
{
    public string FullName { get; set; }

    public string PhoneNumber { get; set; }

    public string Email { get; set; }

    public string EventId { get; set; }

    public string UserId { get; set; }

    public string UserPaymentMethodId { get; set; }

    public List<CreateCheckoutItemDto> Items { get; set; } = new();
}