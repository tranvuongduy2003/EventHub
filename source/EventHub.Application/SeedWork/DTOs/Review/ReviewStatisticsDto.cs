namespace EventHub.Application.SeedWork.DTOs.Review;

public class ReviewStatisticsDto
{
    public float AverageRate { get; set; }

    public int Total { get; set; }

    public int TotalPositive { get; set; }

    public int TotalNegative { get; set; }

    public List<RateStatistic> TotalPerRate { get; set; }
}

public class RateStatistic
{
    public int Rate { get; set; }

    public float Value { get; set; }
}
