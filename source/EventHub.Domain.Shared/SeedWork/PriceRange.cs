using System.ComponentModel;

namespace EventHub.Domain.Shared.SeedWork;

/// <summary>
/// Represents a range of prices with a start and end value.
/// </summary>
/// <remarks>
/// This class is used to define a price range, typically for filtering or querying purposes. It includes properties
/// for the starting and ending values of the range.
/// </remarks>
public class PriceRange
{
    /// <summary>
    /// Gets or sets the starting value of the price range.
    /// </summary>
    /// <value>
    /// A long integer representing the starting value of the price range. Default is 0.
    /// </value>
    [DefaultValue(0)]
    [DisplayName("startRange")]
    [Description("The minimum value of the price range.")]
    public long StartRange { get; set; } = 0;

    /// <summary>
    /// Gets or sets the ending value of the price range.
    /// </summary>
    /// <value>
    /// A long integer representing the ending value of the price range. Default is 0.
    /// </value>
    [DefaultValue(0)]
    [DisplayName("endRange")]
    [Description("The maximum value of the price range.")]
    public long EndRange { get; set; } = 0;
}
