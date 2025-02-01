using System.ComponentModel;
using EventHub.Domain.Shared.Enums.Common;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Domain.Shared.Helpers;

public static class PagingHelper
{
    /// <summary>
    /// Paginates the provided list of items based on the specified pagination filter.
    /// </summary>
    /// <typeparam name="T">The type of the items in the list.</typeparam>
    /// <param name="items">The list of items to paginate.</param>
    /// <param name="filter">The pagination filter containing page number, page size, and sorting criteria.</param>
    /// <returns>A <see cref="Pagination{T}"/> object containing the paginated items and metadata.</returns>
    public static Pagination<T> Paginate<T>(List<T> items, PaginationFilter filter)
    {
        // Get total records of items in database
        int totalCount = items.Count;

        // Retrieve list of items by search values
        if (filter.Searches.Any())
        {
            items = filter.Searches
                .Aggregate(items, (current, search) =>
                    current.Where(x =>
                            search.SearchValue != null
                            && search.SearchBy != null
                            && ((string)TypeDescriptor
                                .GetProperties(typeof(T))
                                .Find(search.SearchBy, true)?
                                .GetValue(x))!
                            .Contains(search.SearchValue, StringComparison.CurrentCultureIgnoreCase))
                        .ToList()
                );
        }

        // Order list of items by order values
        if (filter.Orders.Any())
        {
            items = filter.Orders.Aggregate(items, (current, order) =>
                order.OrderDirection switch
                {
                    EPageOrder.ASC => current.OrderBy(x => TypeDescriptor
                            .GetProperties(typeof(T))
                            .Find(order.OrderBy, true)?
                            .GetValue(x))
                        .ToList(),
                    EPageOrder.DESC => current.OrderByDescending(x => TypeDescriptor
                            .GetProperties(typeof(T))
                            .Find(order.OrderBy, true)?
                            .GetValue(x))
                        .ToList(),
                    _ => current
                });
        }

        // Paging
        if (!filter.TakeAll)
        {
            items = items
                .Skip((filter.Page - 1) * filter.Size)
                .Take(filter.Size)
                .ToList();
        }

        // Summarize data
        var metadata = new Metadata(totalCount, filter.Page, filter.Size, filter.TakeAll);

        return new Pagination<T>
        {
            Items = items,
            Metadata = metadata,
        };
    }

    public static Pagination<T> QueryPaginate<T>(PaginationFilter filter, IQueryable<T> query)
    {
        // Get total records before filtering
        int totalCount = query.Count();

        // Apply text searches
        if (filter.Searches != null && filter.Searches.Any())
        {
            query = query
                .AsEnumerable()
                .Where(x =>
                {
                    bool condition = false;
                    foreach (Search search in filter.Searches)
                    {
                        if (search.SearchValue != null && search.SearchBy != null)
                        {
                            PropertyDescriptor property = TypeDescriptor
                                .GetProperties(typeof(T))
                                .Find(search.SearchBy, true);

                            if (property != null)
                            {
                                condition = condition || ((string)(property.GetValue(x) ?? "")).Contains(search.SearchValue, StringComparison.CurrentCultureIgnoreCase);
                            }
                        }
                    }
                    return condition;
                })
                .AsQueryable();
        }

        // Apply ordering
        if (filter.Orders != null && filter.Orders.Any())
        {
            foreach (Order order in filter.Orders)
            {
                if (order.OrderBy != null)
                {
                    PropertyDescriptor property = TypeDescriptor
                        .GetProperties(typeof(T))
                        .Find(order.OrderBy, true);

                    if (property != null)
                    {

                        query = order.OrderDirection switch
                        {
                            EPageOrder.ASC => query?.OrderBy(x => property.GetValue(x)),
                            EPageOrder.DESC => query?.OrderByDescending(x => property.GetValue(x)),
                            _ => query
                        };
                    }
                }
            }
        }

        // Apply pagination
        var items = new List<T>();
        if (query != null && !filter.TakeAll)
        {
            items = query
                .Skip((filter.Page - 1) * filter.Size)
                .Take(filter.Size)
                .ToList();
        }

        // Create metadata
        var metadata = new Metadata(
            totalCount,
            filter.Page,
            filter.Size,
            filter.TakeAll
        );

        // Return paginated result
        return new Pagination<T>
        {
            Items = items,
            Metadata = metadata
        };
    }
}
