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
    public int Page
    {
        get => _page;
        set => _page = value < 1 ? 1 : value;
    }

    [DefaultValue(10)]
    [DisplayName("size")]
    public int Size
    {
        get => _size;
        set => _size = value < 1 ? 1 : value;
    }
    
    public IEnumerable<Order> Orders
    {
        get => _orders; 
        set => _orders = value;
    }
    
    public IEnumerable<Search> Searches
    {
        get => _searches; 
        set => _searches = value;
    }
    
    [DefaultValue(false)]
    [DisplayName("takeAll")]
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
    public string OrderBy { get; set; }
    
    [DefaultValue(EPageOrder.ASC)]
    [DisplayName("orderDirection")]
    public EPageOrder OrderDirection { get; set; }
}

public class Search
{
    [DefaultValue(null)]
    [DisplayName("searchBy")]
    public string? SearchBy { get; set; }
    
    [DefaultValue(null)]
    [DisplayName("searchValue")]
    public string? SearchValue { get; set; }
}