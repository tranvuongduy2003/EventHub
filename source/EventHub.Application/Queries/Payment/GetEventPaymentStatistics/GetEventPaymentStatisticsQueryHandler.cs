using EventHub.Application.SeedWork.DTOs.Payment;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;

namespace EventHub.Application.Queries.Payment.GetEventPaymentStatistics;

public class GetEventPaymentStatisticsQueryHandler : IQueryHandler<GetEventPaymentStatisticsQuery, PaymentStatisticsDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetEventPaymentStatisticsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<PaymentStatisticsDto> Handle(GetEventPaymentStatisticsQuery request,
        CancellationToken cancellationToken)
    {
        int total = _unitOfWork.Payments.FindByCondition(x => x.EventId == request.EventId).Count();
        int totalSuccess = _unitOfWork.Payments.FindByCondition(x => x.EventId == request.EventId).Count(x => x.Status == Domain.Shared.Enums.Payment.EPaymentStatus.SUCCESS);
        int totalFailed = _unitOfWork.Payments.FindByCondition(x => x.EventId == request.EventId).Count(x => x.Status == Domain.Shared.Enums.Payment.EPaymentStatus.FAILED);
        int totalPending = _unitOfWork.Payments.FindByCondition(x => x.EventId == request.EventId).Count(x => x.Status == Domain.Shared.Enums.Payment.EPaymentStatus.PENDING);

        return Task.FromResult(new PaymentStatisticsDto
        {
            Total = total,
            TotalFailed = totalFailed,
            TotalPending = totalPending,
            TotalSuccess = totalSuccess,
        });
    }
}
