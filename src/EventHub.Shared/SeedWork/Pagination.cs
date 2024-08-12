namespace EventHub.Shared.SeedWork;

public class Pagination<T>
{
    public List<T> Items { get; set; }

    public Metadata Metadata { get; set; }
}

public class Pagination<T, K>
{
    public List<T> Items { get; set; }

    public K Metadata { get; set; }
}