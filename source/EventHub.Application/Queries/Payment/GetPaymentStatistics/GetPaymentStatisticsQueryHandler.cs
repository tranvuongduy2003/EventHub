using EventHub.Application.SeedWork.DTOs.Payment;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;

namespace EventHub.Application.Queries.Payment.GetPaymentStatistics;

public class GetPaymentStatisticsQueryHandler : IQueryHandler<GetPaymentStatisticsQuery, PaymentStatisticsDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPaymentStatisticsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<PaymentStatisticsDto> Handle(GetPaymentStatisticsQuery request,
        CancellationToken cancellationToken)
    {
        int total = _unitOfWork.Payments.FindAll().Count();
        int totalSuccess = _unitOfWork.Payments.FindAll().Count(x => x.Status == Domain.Shared.Enums.Payment.EPaymentStatus.SUCCESS);
        int totalFailed = _unitOfWork.Payments.FindAll().Count(x => x.Status == Domain.Shared.Enums.Payment.EPaymentStatus.FAILED);
        int totalPending = _unitOfWork.Payments.FindAll().Count(x => x.Status == Domain.Shared.Enums.Payment.EPaymentStatus.PENDING);

        return Task.FromResult(new PaymentStatisticsDto
        {
            Total = total,
            TotalFailed = totalFailed,
            TotalPending = totalPending,
            TotalSuccess = totalSuccess,
        });
    }
}
