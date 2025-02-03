namespace EventHub.Application.SeedWork.DTOs.Payment;

public class CheckoutDto
{
    public string CustomerName { get; set; }

    public string CustomerEmail { get; set; }

    public string CustomerPhone { get; set; }

    public Guid EventId { get; set; }

    public Guid UserId { get; set; }

    public long TotalPrice { get; set; }

    public List<CheckoutItemDto> CheckoutItems { get; set; } = new();

    public Guid? CouponId { get; set; } = null!;

    public string SuccessUrl { get; set; }

    public string CancelUrl { get; set; }
}
