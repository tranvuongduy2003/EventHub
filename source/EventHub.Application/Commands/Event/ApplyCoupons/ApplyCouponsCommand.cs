using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Event.ApplyCoupons;

public record ApplyCouponsCommand(Guid EventId, List<Guid> CouponIds) : ICommand;
