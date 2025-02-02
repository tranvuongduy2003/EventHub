namespace EventHub.Application.SeedWork.DTOs.Event;

public class CreatedEventsStatisticsDto
{
    public int TotalEvents { get; set; }

    public int TotalPublicEvents { get; set; }

    public int TotalPrivateEvents { get; set; }
}
