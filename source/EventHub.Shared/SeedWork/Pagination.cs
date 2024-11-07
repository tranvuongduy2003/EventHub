namespace EventHub.Shared.SeedWork;

/// <summary>
/// Represents a paginated result containing a list of items and metadata about the pagination.
/// </summary>
/// <typeparam name="T">The type of the items in the paginated list.</typeparam>
/// <remarks>
/// This class is used to encapsulate the result of a paginated query, including the list of items and pagination metadata.
/// </remarks>
public class Pagination<T>
{
    /// <summary>
    /// Gets or sets the list of items in the current page.
    /// </summary>
    /// <value>
    /// A list of items of type <typeparamref name="T"/> that represents the items on the current page.
    /// </value>
    public List<T> Items { get; set; }

    /// <summary>
    /// Gets or sets the metadata about the pagination.
    /// </summary>
    /// <value>
    /// An instance of <see cref="Metadata"/> containing information about the pagination, such as total items, total pages, etc.
    /// </value>
    public Metadata Metadata { get; set; }
}

/// <summary>
/// Represents a paginated result containing a list of items and custom metadata about the pagination.
/// </summary>
/// <typeparam name="T">The type of the items in the paginated list.</typeparam>
/// <typeparam name="K">The type of the metadata associated with the pagination.</typeparam>
/// <remarks>
/// This class is used to encapsulate the result of a paginated query, including the list of items and custom metadata about the pagination.
/// </remarks>
public class Pagination<T, K>
{
    /// <summary>
    /// Gets or sets the list of items in the current page.
    /// </summary>
    /// <value>
    /// A list of items of type <typeparamref name="T"/> that represents the items on the current page.
    /// </value>
    public List<T> Items { get; set; }

    /// <summary>
    /// Gets or sets the custom metadata about the pagination.
    /// </summary>
    /// <value>
    /// An instance of <typeparamref name="K"/> containing custom metadata about the pagination.
    /// </value>
    public K Metadata { get; set; }
}