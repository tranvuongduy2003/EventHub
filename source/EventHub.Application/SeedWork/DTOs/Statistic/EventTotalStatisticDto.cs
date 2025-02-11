namespace EventHub.Application.SeedWork.DTOs.Statistic;

public class EventTotalStatisticDto
{
    public int TotalTickets { get; set; }

    public int TotalOrders { get; set; }

    public long TotalRevenues { get; set; }

    public long TotalExpenses { get; set; }

    public long TotalProfits { get; set; }
}
