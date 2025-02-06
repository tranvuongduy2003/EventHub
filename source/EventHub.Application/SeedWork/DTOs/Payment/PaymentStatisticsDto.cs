namespace EventHub.Application.SeedWork.DTOs.Payment;

public class PaymentStatisticsDto
{
    public int Total { get; set; }

    public int TotalPending { get; set; }

    public int TotalSuccess { get; set; }

    public int TotalFailed { get; set; }
}
