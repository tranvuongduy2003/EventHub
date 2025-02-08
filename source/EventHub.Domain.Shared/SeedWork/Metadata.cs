using System.ComponentModel;

namespace EventHub.Domain.Shared.SeedWork;

/// <summary>
/// Represents metadata about the pagination of a query result.
/// </summary>
/// <remarks>
/// This class provides information about the current page, total pages, page size, total item count, and whether all items are being retrieved. 
/// It also includes properties to determine if there are pages before or after the current page.
/// </remarks>
public class Metadata
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Metadata"/> class with default values.
    /// </summary>
    public Metadata()
    {
        TotalCount = 0;
        PayloadSize = 0;
        PageSize = 10;
        CurrentPage = 1;
        TotalPages = 0;
        TakeAll = true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Metadata"/> class with specified values.
    /// </summary>
    /// <param name="count">The total number of items available.</param>
    /// <param name="pageNumber">The current page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="takeAll">A flag indicating whether to retrieve all items without pagination.</param>
    public Metadata(int count, int pageNumber, int pageSize, bool takeAll)
    {
        TotalCount = count;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TakeAll = takeAll;
        TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
        PayloadSize = Math.Min(PageSize, TotalCount - (CurrentPage - 1) * PageSize);
    }

    /// <summary>
    /// Gets the current page number.
    /// </summary>
    /// <value>
    /// An integer representing the current page number. Default value is 1.
    /// </value>
    [DefaultValue(1)]
    [Description("Current page number")]
    public int CurrentPage { get; }

    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    /// <value>
    /// An integer representing the total number of pages. Default value is 0.
    /// </value>
    [DefaultValue(0)]
    [Description("Total pages")]
    public int TotalPages { get; }

    /// <summary>
    /// Gets a value indicating whether all items are being retrieved without pagination.
    /// </summary>
    /// <value>
    /// A boolean indicating whether to skip paging and get all items. Default value is false.
    /// </value>
    [DefaultValue(false)]
    [Description("If TakeAll equals true, skip paging and get all items")]
    public bool TakeAll { get; private set; }

    /// <summary>
    /// Gets the number of items per page.
    /// </summary>
    /// <value>
    /// An integer representing the number of items per page. Default value is 10.
    /// </value>
    [DefaultValue(10)]
    [Description("Total items of each page")]
    public int PageSize { get; private set; }

    /// <summary>
    /// Gets the total number of items available.
    /// </summary>
    /// <value>
    /// An integer representing the total number of items. Default value is 0.
    /// </value>
    [DefaultValue(0)]
    [Description("Total items")]
    public int TotalCount { get; private set; }

    /// <summary>
    /// Gets the number of items on the current page.
    /// </summary>
    /// <value>
    /// An integer representing the number of items on the current page. Default value is 0.
    /// </value>
    [DefaultValue(0)]
    [Description("Total items in the current page")]
    public int PayloadSize { get; private set; }

    /// <summary>
    /// Gets a value indicating whether there is a page before the current page.
    /// </summary>
    /// <value>
    /// A boolean value indicating if there is a page before the current page. Default value is false.
    /// </value>
    [DefaultValue(false)]
    [Description("Existing a page in front of the current page or not")]
    public bool HasPrevious => CurrentPage > 1;

    /// <summary>
    /// Gets a value indicating whether there is a page after the current page.
    /// </summary>
    /// <value>
    /// A boolean value indicating if there is a page after the current page. Default value is false.
    /// </value>
    [DefaultValue(false)]
    [Description("Existing a page behind the current page or not")]
    public bool HasNext => CurrentPage < TotalPages;
}

public class NotificationMetadata
{
    public int TotalUnseenFollowing { get; set; }

    public int TotalUnseenOrdering { get; set; }

    public int TotalUnseenInvitation { get; set; }
}
