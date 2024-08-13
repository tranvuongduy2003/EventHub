using EventHub.Shared.Enums.Common;
using EventHub.Shared.SeedWork;

namespace EventHub.Shared.Helpers;

public static class PagingHelper
{
    public static Pagination<T> Paginate<T>(List<T> items, PaginationFilter filter)
    {
        if (filter.Searches.Any())
        {
            items = filter.Searches
                .Aggregate(items, (current, search) =>
                    (List<T>)current.Where(x =>
                        search.SearchValue != null
                        && search.SearchBy != null
                        && x.GetType().GetProperty(search.SearchBy)
                            .ToString()
                            .Contains(search.SearchValue, StringComparison.CurrentCultureIgnoreCase)));
        }

        if (filter.Orders.Any())
        {
            items = filter.Orders.Aggregate(items, (current, order) => 
                order.OrderDirection switch
                {
                    EPageOrder.ASC => (List<T>)current.OrderBy(x => x.GetType().GetProperty(order.OrderBy)),
                    EPageOrder.DESC => (List<T>)current.OrderByDescending(x => x.GetType().GetProperty(order.OrderBy)),
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