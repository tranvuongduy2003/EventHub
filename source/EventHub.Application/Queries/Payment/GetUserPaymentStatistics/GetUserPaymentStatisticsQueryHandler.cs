using EventHub.Application.SeedWork.DTOs.Payment;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;

namespace EventHub.Application.Queries.Payment.GetUserPaymentStatistics;

public class GetUserPaymentStatisticsQueryHandler : IQueryHandler<GetUserPaymentStatisticsQuery, PaymentStatisticsDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserPaymentStatisticsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<PaymentStatisticsDto> Handle(GetUserPaymentStatisticsQuery request,
        CancellationToken cancellationToken)
    {
        int total = _unitOfWork.Payments.FindByCondition(x => x.AuthorId == request.UserId).Count();
        int totalSuccess = _unitOfWork.Payments.FindByCondition(x => x.AuthorId == request.UserId).Count(x => x.Status == Domain.Shared.Enums.Payment.EPaymentStatus.SUCCESS);
        int totalFailed = _unitOfWork.Payments.FindByCondition(x => x.AuthorId == request.UserId).Count(x => x.Status == Domain.Shared.Enums.Payment.EPaymentStatus.FAILED);
        int totalPending = _unitOfWork.Payments.FindByCondition(x => x.AuthorId == request.UserId).Count(x => x.Status == Domain.Shared.Enums.Payment.EPaymentStatus.PENDING);

        return Task.FromResult(new PaymentStatisticsDto
        {
            Total = total,
            TotalFailed = totalFailed,
            TotalPending = totalPending,
            TotalSuccess = totalSuccess,
        });
    }
}
