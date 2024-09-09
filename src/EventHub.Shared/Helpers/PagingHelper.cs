using System.ComponentModel;
using EventHub.Shared.Enums.Common;
using EventHub.Shared.SeedWork;

namespace EventHub.Shared.Helpers;

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
        if (filter.Searches.Any())
        {
            items = filter.Searches
                .Aggregate(items, (current, search) =>
                    (List<T>)current.Where(x =>
                        search.SearchValue != null
                        && search.SearchBy != null
                        && ((string)TypeDescriptor
                            .GetProperties(typeof(T))
                            .Find(search.SearchBy, true)?
                            .GetValue(x))
                            .Contains(search.SearchValue, StringComparison.CurrentCultureIgnoreCase)));
        }

        if (filter.Orders.Any())
        {
            items = filter.Orders.Aggregate(items, (current, order) =>
                order.OrderDirection switch
                {
                    EPageOrder.ASC => (List<T>)current.OrderBy(x => TypeDescriptor
                        .GetProperties(typeof(T))
                        .Find(order.OrderBy, true)?
                        .GetValue(x)),
                    EPageOrder.DESC => (List<T>)current.OrderByDescending(x => TypeDescriptor
                        .GetProperties(typeof(T))
                        .Find(order.OrderBy, true)?
                        .GetValue(x)),
                    _ => current
                });
        }

        if (filter.TakeAll == false)
        {
            items = items
                .Skip((filter.Page - 1) * filter.Size)
                .Take(filter.Size)
                .ToList();
        }

        var metadata = new Metadata(items.Count, filter.Page, filter.Size, filter.TakeAll);

        return new Pagination<T>
        {
            Items = items,
            Metadata = metadata,
        };
    }
}