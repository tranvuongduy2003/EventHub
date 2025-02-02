using EventHub.Application.SeedWork.DTOs.Payment;
using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Payment.Checkout;

public class CheckoutCommand : ICommand<CheckoutResponseDto>
{
    public CheckoutCommand(CheckoutDto request)
    {
        CustomerName = request.CustomerName;
        CustomerEmail = request.CustomerEmail;
        CustomerPhone = request.CustomerPhone;
        EventId = request.EventId;
        UserId = request.UserId;
        TotalPrice = request.TotalPrice;
        CheckoutItems = request.CheckoutItems
            .Select(x => new CheckoutItemCommand
            {
                Name = x.Name,
                EventId = request.EventId,
                Price = x.Price,
                Quantity = x.Quantity,
                TicketTypeId = x.TicketTypeId,
            })
            .ToList();
        CouponId = request.CouponId;
        SuccessUrl = request.SuccessUrl;
        CancelUrl = request.CancelUrl;
    }

    public string CustomerName { get; set; }

    public string CustomerEmail { get; set; }

    public string CustomerPhone { get; set; }

    public Guid EventId { get; set; }

    public Guid UserId { get; set; }

    public long TotalPrice { get; set; }

    public List<CheckoutItemCommand> CheckoutItems { get; set; }

    public Guid CouponId { get; set; }

    public string SuccessUrl { get; set; }

    public string CancelUrl { get; set; }
}
