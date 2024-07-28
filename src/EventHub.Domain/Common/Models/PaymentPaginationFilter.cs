using System.ComponentModel;
using EventHub.Domain.Enums.Payment;

namespace EventHub.Domain.Common.Models;

public class PaymentPaginationFilter : PaginationFilter
{
    [DefaultValue(EPaymentStatus.ALL)] public EPaymentStatus? status { get; set; } = EPaymentStatus.ALL;
}