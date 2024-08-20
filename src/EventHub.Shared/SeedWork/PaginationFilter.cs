using System.ComponentModel;
using EventHub.Shared.Enums.Common;

namespace EventHub.Shared.SeedWork;

public class PaginationFilter
{
    private int _page = 1;

    private int _size = 10;

    private IEnumerable<Order> _orders = new List<Order>();
    
    private IEnumerable<Search> _searches = new List<Search>();

    private bool _takeAll = false;
    

    [DefaultValue(1)]
    [DisplayName("page")]
    [Description("Current page number")]
    public int Page
    {
        get => _page;
        set => _page = value < 1 ? 1 : value;
    }

    [DefaultValue(10)]
    [DisplayName("size")]
    [Description("Total items of each page")]
    public int Size
    {
        get => _size;
        set => _size = value < 1 ? 1 : value;
    }
    
    [DisplayName("orders")]
    [Description("The list pairs of the ordered attribute and its order direction")]
    public IEnumerable<Order> Orders
    {
        get => _orders; 
        set => _orders = value;
    }
    
    [DisplayName("searches")]
    [Description("The list pairs of the searched attribute and its search value")]
    public IEnumerable<Search> Searches
    {
        get => _searches; 
        set => _searches = value;
    }
    
    [DefaultValue(false)]
    [DisplayName("takeAll")]
    [Description("If takeAll equals true, skip paging and get all items")]
    public bool TakeAll
    {
        get => _takeAll; 
        set => _takeAll = value;
    }
}

public class Order
{
    [DefaultValue(null)]
    [DisplayName("orderBy")]
    [Description("Name of the ordered attribute")]
    public string OrderBy { get; set; }
    
    [DefaultValue(EPageOrder.ASC)]
    [DisplayName("orderDirection")]
    [Description("Direction of the ordered attribute (ASC, DESC)")]
    public EPageOrder OrderDirection { get; set; }
}

public class Search
{
    [DefaultValue(null)]
    [DisplayName("searchBy")]
    [Description("Name of the searched attribute")]
    public string? SearchBy { get; set; }
    
    [DefaultValue(null)]
    [DisplayName("searchValue")]
    [Description("Value of the searched attribute")]
    public string? SearchValue { get; set; }
}