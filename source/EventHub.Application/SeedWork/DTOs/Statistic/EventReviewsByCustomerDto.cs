namespace EventHub.Application.SeedWork.DTOs.Statistic;

public class EventReviewsByCustomerDto
{
    public int TotalPositive { get; set; }

    public int TotalNegative { get; set; }

    public int TotalReviews { get; set; }

    public double AvarageRate { get; set; }
}
