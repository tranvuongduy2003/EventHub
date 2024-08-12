using EventHub.Shared.Enums.Event;
using EventHub.Shared.Models.Category;
using EventHub.Shared.SeedWork;

namespace EventHub.Shared.Models.Event;

public class EventModel
{
    public string Id { get; set; }

    public string CreatorName { get; set; }

    public string CreatorAvatar { get; set; }

    public string CoverImage { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Location { get; set; }

    public PriceRange PriceRange { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public EEventCycleType EventCycleType { get; set; }

    public EEventPaymentType EventPaymentType { get; set; }

    public bool IsPrivate { get; set; }

    public bool IsTrash { get; set; }

    public List<CategoryModel> Categories { get; set; }

    public double AverageRating { get; set; } = 0.0;

    public double Promotion { get; set; } = 0;

    public EEventStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}