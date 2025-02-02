using EventHub.Application.SeedWork.DTOs.Coupon;
using EventHub.Domain.SeedWork.Command;
using Microsoft.AspNetCore.Http;

namespace EventHub.Application.Commands.Coupon.UpdateCoupon;

/// <summary>
/// Represents a command to update an existing coupon.
/// </summary>
/// <remarks>
/// This command is used to request an update to an existing coupon specified by its unique identifier.
/// </remarks>
public class UpdateCouponCommand : ICommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateCouponCommand"/> class.
    /// </summary>
    /// <param name="id">
    /// The unique identifier of the coupon to be updated.
    /// </param>
    /// <param name="request">
    /// The data transfer object containing the updated details of the coupon.
    /// </param>
    public UpdateCouponCommand(Guid id, UpdateCouponDto request)
    {
        Id = id;
        Name = request.Name;
        Name = request.Name;
        Description = request.Name;
        MinQuantity = request.Quantity;
        MinPrice = request.MinPrice;
        PercentValue = request.PercentValue;
        ExpiredDate = request.ExpiredDate;
        CoverImage = request.CoverImage;
    }

    /// <summary>
    /// Gets or sets the unique identifier of the coupon to be updated.
    /// </summary>
    /// <value>
    /// A string representing the unique identifier of the coupon.
    /// </value>
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int MinQuantity { get; set; }

    public long MinPrice { get; set; }

    public float PercentValue { get; set; }

    public DateTime ExpiredDate { get; set; }

    public IFormFile CoverImage { get; set; }
}
