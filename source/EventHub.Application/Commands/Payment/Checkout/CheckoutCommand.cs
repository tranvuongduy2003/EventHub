using EventHub.Application.SeedWork.DTOs.Payment;
using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Payment.Checkout;

public class CheckoutCommand : ICommand<PaymentDto>
{
    public CheckoutCommand(CheckoutDto request)
    {
        CustomerName = request.CustomerName;
        CustomerEmail = request.CustomerEmail;
        CustomerPhone = request.CustomerPhone;
        EventId = request.EventId;
        UserId = request.UserId;
        Discount = request.Discount;
        TotalPrice = request.TotalPrice;
        CheckoutItems = request.CheckoutItems
            .Select(x => new CheckoutItemCommand
            {
                Name = x.Name,
                EventId = x.EventId,
                TotalPrice = x.TotalPrice,
                Quantity = x.Quantity,
                TicketTypeId = x.TicketTypeId,
            })
            .ToList();
        CouponIds = request.CouponIds;
    }

    public string CustomerName { get; set; }

    public string CustomerEmail { get; set; }

    public string CustomerPhone { get; set; }

    public Guid EventId { get; set; }

    public Guid UserId { get; set; }

    public double Discount { get; set; }

    public long TotalPrice { get; set; }

    public List<CheckoutItemCommand> CheckoutItems { get; set; }

    public List<Guid> CouponIds { get; set; }
}
