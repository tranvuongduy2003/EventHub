using System.ComponentModel;

namespace EventHub.Shared.SeedWork;

public class Metadata
{
    public Metadata()
    {
        TotalCount = 0;
        PayloadSize = 0;
        PageSize = 10;
        CurrentPage = 1;
        TotalPages = 0;
        TakeAll = true;
    }

    public Metadata(int count, int pageNumber, int pageSize, bool takeAll)
    {
        TotalCount = count;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TakeAll = takeAll;
        TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
        PayloadSize = Math.Min(PageSize, TotalCount - (CurrentPage - 1) * PageSize);
    }

    [DefaultValue(1)]
    [Description("Current page number")]
    public int CurrentPage { get; }

    [DefaultValue(0)]
    [Description("Total pages")]
    public int TotalPages { get; }

    [DefaultValue(false)]
    [Description("If TakeAll equals true, skip paging and get all items")]
    public bool TakeAll { get; private set; }

    [DefaultValue(10)]
    [Description("Total items of each page")]
    public int PageSize { get; private set; }

    [DefaultValue(0)]
    [Description("Total items")]
    public int TotalCount { get; private set; }

    [DefaultValue(0)]
    [Description("Total items in the current page")]
    public int PayloadSize { get; private set; }

    [DefaultValue(false)]
    [Description("Existing a page in front of the current page or not")]
    public bool HasPrevious => CurrentPage > 1;

    [DefaultValue(false)]
    [Description("Existing a page behind the current page or not")]
    public bool HasNext => CurrentPage < TotalPages;
}