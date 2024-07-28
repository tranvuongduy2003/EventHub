using System.ComponentModel;
using EventHub.Domain.Enums.Common;

namespace EventHub.Domain.Common.Models;

public class PaginationFilter
{
    private int _page = 1;

    public string? _search;


    private int _size = 10;

    [DefaultValue(1)]
    public int page
    {
        get => _page;
        set => _page = value < 1 ? 1 : value;
    }

    [DefaultValue(10)]
    public int size
    {
        get => _size;
        set => _size = value < 1 ? 1 : value;
    }

    [DefaultValue(true)] public bool takeAll { get; set; } = true;

    public EPageOrder order { get; set; } = EPageOrder.ASC;

    [DefaultValue(null)]
    public string? search
    {
        get => _search;
        set => _search = value;
    }
}