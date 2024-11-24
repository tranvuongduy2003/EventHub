using System.ComponentModel;
using EventHub.Shared.Enums.Common;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.SeedWork;

/// <summary>
/// Represents the filter parameters used for pagination, ordering, and searching in a paginated data query.
/// </summary>
/// <remarks>
/// This class includes parameters to control pagination (page number and page size), ordering of results, 
/// and searching through attributes. It also includes an option to retrieve all items without pagination.
/// </remarks>
public class PaginationFilter
{
    private List<Order> _orders = new List<Order>();
    private int _page = 1;
    private List<Search> _searches = new List<Search>();
    private int _size = 10;


    /// <summary>
    /// Gets or sets the current page number for pagination.
    /// </summary>
    /// <value>
    /// An integer representing the current page number. The default value is 1. The value is clamped to be at least 1.
    /// </value>
    [DefaultValue(1)]
    [SwaggerSchema("Current page number")]
    [FromQuery(Name = "page")]
    public int Page
    {
        get => _page;
        set => _page = value < 1 ? 1 : value;
    }

    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    /// <value>
    /// An integer representing the total items per page. The default value is 10. The value is clamped to be at least 1.
    /// </value>
    [DefaultValue(10)]
    [SwaggerSchema("Total items of each page")]
    [FromQuery(Name = "size")]
    public int Size
    {
        get => _size;
        set => _size = value < 1 ? 1 : value;
    }

    /// <summary>
    /// Gets or sets the list of ordering criteria for the results.
    /// </summary>
    /// <value>
    /// A collection of <see cref="Order"/> instances representing the attributes to order by and their respective directions.
    /// </value>
    [SwaggerParameter("The list pairs of the ordered attribute and its order direction")]
    [FromQuery(Name = "orders")]
    public IEnumerable<Order> Orders
    {
        get => _orders;
        set => _orders = value.ToList();
    }

    /// <summary>
    /// Gets or sets the list of searching criteria for the results.
    /// </summary>
    /// <value>
    /// A collection of <see cref="Search"/> instances representing the attributes to search by and their respective values.
    /// </value>
    [SwaggerSchema("The list pairs of the searched attribute and its search value")]
    [FromQuery(Name = "searches")]
    public IEnumerable<Search> Searches
    {
        get => _searches;
        set => _searches = value.ToList();
    }

    /// <summary>
    /// Gets or sets a flag indicating whether to retrieve all items without pagination.
    /// </summary>
    /// <value>
    /// A boolean value indicating whether to skip paging and get all items. The default value is false.
    /// </value>
    [DefaultValue(false)]
    [SwaggerSchema("If takeAll equals true, skip paging and get all items")]
    [FromQuery(Name = "takeAll")]
    public bool TakeAll { get; set; }
}

/// <summary>
/// Represents an ordering criteria for a paginated query.
/// </summary>
/// <remarks>
/// This class specifies the attribute to order by and the direction of the order.
/// </remarks>
public class Order
{
    /// <summary>
    /// Gets or sets the name of the attribute to order by.
    /// </summary>
    /// <value>
    /// A string representing the name of the attribute to order by. The default value is null.
    /// </value>
    [DefaultValue(null)]
    public string OrderBy { get; set; }

    /// <summary>
    /// Gets or sets the direction of the order for the attribute.
    /// </summary>
    /// <value>
    /// An enumeration value of type <see cref="EPageOrder"/> representing the direction of the order. The default value is <see cref="EPageOrder.ASC"/>.
    /// </value>
    [DefaultValue(EPageOrder.ASC)]
    public EPageOrder OrderDirection { get; set; }
}

/// <summary>
/// Represents a search criteria for a paginated query.
/// </summary>
/// <remarks>
/// This class specifies the attribute to search by and the search value for filtering results.
/// </remarks>
public class Search
{
    /// <summary>
    /// Gets or sets the name of the attribute to search by.
    /// </summary>
    /// <value>
    /// A nullable string representing the name of the attribute to search by. The default value is null.
    /// </value>
    [DefaultValue(null)]
    public string? SearchBy { get; set; }

    /// <summary>
    /// Gets or sets the value to search for in the specified attribute.
    /// </summary>
    /// <value>
    /// A nullable string representing the search value for the attribute. The default value is null.
    /// </value>
    [DefaultValue(null)]
    public string? SearchValue { get; set; }
}
