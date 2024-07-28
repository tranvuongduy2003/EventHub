namespace EventHub.Domain.Common.Models;

public class Metadata
{
    public Metadata()
    {
        TotalCount = 0;
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
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TakeAll = takeAll;
    }

    public int CurrentPage { get; }

    public int TotalPages { get; }

    public bool TakeAll { get; private set; }

    public int PageSize { get; private set; }

    public int TotalCount { get; private set; }

    public bool HasPrevious => CurrentPage > 1;

    public bool HasNext => CurrentPage < TotalPages;
}