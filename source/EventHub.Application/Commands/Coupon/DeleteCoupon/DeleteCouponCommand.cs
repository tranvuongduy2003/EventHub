using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Coupon.DeleteCoupon;

/// <summary>
/// Represents a command to delete a coupon by its unique identifier.
/// </summary>
/// <remarks>
/// This command is used to request the deletion of a coupon specified by its unique identifier.
/// </remarks>
public record DeleteCouponCommand(Guid CouponId) : ICommand;
