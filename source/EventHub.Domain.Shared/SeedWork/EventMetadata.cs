namespace EventHub.Domain.Shared.SeedWork;

public class EventMetadata : Metadata
{
    public EventMetadata()
    {
        TotalPublic = 0;
        TotalPrivate = 0;
        TotalTrash = 0;
    }

    public EventMetadata(int count, int pageNumber, int pageSize, bool takeAll) : base(count, pageNumber, pageSize,
        takeAll)
    {
    }

    public EventMetadata(int count, int pageNumber, int pageSize, bool takeAll, int totalTrash) : base(count,
        pageNumber, pageSize, takeAll)
    {
        TotalTrash = totalTrash;
    }

    public EventMetadata(int count, int pageNumber, int pageSize, bool takeAll, int totalPublic, int totalPrivate) :
        base(count, pageNumber, pageSize, takeAll)
    {
        TotalPublic = totalPublic;
        TotalPrivate = totalPrivate;
    }

    public EventMetadata(int count, int pageNumber, int pageSize, bool takeAll, int totalPublic, int totalPrivate,
        int totalTrash) : base(count, pageNumber, pageSize, takeAll)
    {
        TotalPublic = totalPublic;
        TotalPrivate = totalPrivate;
        TotalTrash = totalTrash;
    }

    public int TotalPublic { get; private set; }

    public int TotalPrivate { get; private set; }

    public int TotalTrash { get; private set; }
}
