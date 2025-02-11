using EventHub.Application.SeedWork.DTOs.Statistic;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;

namespace EventHub.Application.Queries.Event.GetEventTotalStatistic;

public class GetEventTotalStatisticQueryHandler : IQueryHandler<GetEventTotalStatisticQuery, EventTotalStatisticDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetEventTotalStatisticQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<EventTotalStatisticDto> Handle(GetEventTotalStatisticQuery request,
        CancellationToken cancellationToken)
    {
        int totalTickets = _unitOfWork.Tickets.FindByCondition(x => x.EventId == request.EventId).Count();

        int totalOrders = _unitOfWork.Payments.FindByCondition(x => x.EventId == request.EventId).Count();

        long totalRevenues = _unitOfWork.Payments
            .FindByCondition(x =>
                x.EventId == request.EventId
                && x.Status == Domain.Shared.Enums.Payment.EPaymentStatus.SUCCESS)
            .Sum(x => x.TotalPrice - x.Discount);

        long totalExpenses = _unitOfWork.Expenses.FindByCondition(x => x.EventId == request.EventId).Sum(x => x.Total);

        long totalProfits = totalRevenues - totalExpenses;

        return Task.FromResult(new EventTotalStatisticDto
        {
            TotalExpenses = totalExpenses,
            TotalOrders = totalOrders,
            TotalProfits = totalProfits,
            TotalRevenues = totalRevenues,
            TotalTickets = totalTickets
        });
    }
}
