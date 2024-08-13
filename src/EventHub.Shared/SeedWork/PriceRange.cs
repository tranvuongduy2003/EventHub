using System.ComponentModel;

namespace EventHub.Shared.SeedWork;

public class PriceRange
{
    [DefaultValue(0)]
    [DisplayName("startRange")]
    public long StartRange { get; set; } = 0;

    [DefaultValue(0)]
    [DisplayName("endRange")]
    public long EndRange { get; set; } = 0;
}