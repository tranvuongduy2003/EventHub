using EventHub.Domain.Enums.Event;
using EventHub.Domain.Models.User;

namespace EventHub.Domain.Models.Event;

public class EventDetailModel
{
    public string Id { get; set; }

    public string CreatorId { get; set; }

    public AuthorModel Author { get; set; }

    public string CoverImage { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Location { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public List<string> CategoryIds { get; set; } = new();

    public List<string> SubImages { get; set; } = new();

    public List<string> Reasons { get; set; } = new();

    public double Promotion { get; set; } = 0;

    public EEventCycleType EventCycleType { get; set; }

    public EEventPaymentType EventPaymentType { get; set; }

    public List<TicketTypeModel> TicketTypes { get; set; }

    public double AverageRating { get; set; } = 0.0;

    public EmailContentModel EmailContent { get; set; }

    public bool? IsFavourite { get; set; } = false;

    public bool? IsPrivate { get; set; } = false;

    public bool? IsTrash { get; set; } = false;

    public int? NumberOfFavourites { get; set; } = 0;

    public int? NumberOfShares { get; set; } = 0;

    public int? NumberOfSoldTickets { get; set; } = 0;

    public EEventStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}