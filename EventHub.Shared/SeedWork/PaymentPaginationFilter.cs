using System.ComponentModel;
using EventHub.Shared.Enums.Payment;

namespace EventHub.Shared.SeedWork;

public class PaymentPaginationFilter : PaginationFilter
{
    [DefaultValue(EPaymentStatus.ALL)] public EPaymentStatus? status { get; set; } = EPaymentStatus.ALL;
}