using System.ComponentModel;
using EventHub.Domain.Enums.Event;

namespace EventHub.Domain.Common.Models;

public class EventPaginationFilter : PaginationFilter
{
    private List<double> _rates;

    [DefaultValue(EEventType.ALL)] public EEventType type { get; set; } = EEventType.ALL;

    [DefaultValue(null)] public string? location { get; set; }

    [DefaultValue(null)] public PriceRange? priceRange { get; set; }

    [DefaultValue(null)] public List<string>? categoryIds { get; set; }

    [DefaultValue(EEventPrivacy.ALL)] public EEventPrivacy? eventPrivacy { get; set; } = EEventPrivacy.ALL;

    [DefaultValue(null)]
    public List<double>? rates
    {
        get => _rates;
        set => _rates = value;
    }
}