using EventHub.Application.SeedWork.DTOs.Payment;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Payment.GetCreatedEventsPaymentStatistics;

public class GetCreatedEventsPaymentStatisticsQueryHandler : IQueryHandler<GetCreatedEventsPaymentStatisticsQuery, PaymentStatisticsDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCreatedEventsPaymentStatisticsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<PaymentStatisticsDto> Handle(GetCreatedEventsPaymentStatisticsQuery request,
        CancellationToken cancellationToken)
    {
        var payments = _unitOfWork.Payments
            .FindAll()
            .Include(x => x.Event)
            .Where(x => x.Event.AuthorId == request.AuthorId)
            .ToList();

        int total = payments.Count;
        int totalSuccess = payments.Count(x => x.Status == Domain.Shared.Enums.Payment.EPaymentStatus.SUCCESS);
        int totalFailed = payments.Count(x => x.Status == Domain.Shared.Enums.Payment.EPaymentStatus.FAILED);
        int totalPending = payments.Count(x => x.Status == Domain.Shared.Enums.Payment.EPaymentStatus.PENDING);

        return Task.FromResult(new PaymentStatisticsDto
        {
            Total = total,
            TotalFailed = totalFailed,
            TotalPending = totalPending,
            TotalSuccess = totalSuccess,
        });
    }
}
