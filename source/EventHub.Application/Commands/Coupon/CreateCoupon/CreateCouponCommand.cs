using EventHub.Application.SeedWork.DTOs.Coupon;
using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Coupon.CreateCoupon;

/// <summary>
/// Represents a command to create a new coupon.
/// </summary>
/// <remarks>
/// This command is used to create a new coupon with the specified details.
/// </remarks>
public class CreateCouponCommand : ICommand<CouponDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateCouponCommand"/> class.
    /// </summary>
    public CreateCouponCommand()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateCouponCommand"/> class.
    /// </summary>
    /// <param name="request">
    /// The data transfer object containing the details for the new coupon.
    /// </param>
    public CreateCouponCommand(CreateCouponDto request)
        => (Name, Description, MinQuantity, MinPrice, PercentValue, ExpiredDate) =
            (request.Name, request.Description, request.Quantity, request.MinPrice, request.PercentValue,
                request.ExpiredDate);

    public string Name { get; set; }

    public string Description { get; set; }

    public int MinQuantity { get; set; }

    public long MinPrice { get; set; }

    public float PercentValue { get; set; }

    public DateTime ExpiredDate { get; set; }
}
