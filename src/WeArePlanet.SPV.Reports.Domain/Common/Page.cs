namespace WeArePlanet.SPV.Reports.Domain.Common;

public class Page<T>
{
    public Page(IReadOnlyList<T> entries, int pageNumber, int pageSize, long totalItems)
    {
        this.Entries = entries ?? throw new ArgumentNullException(nameof(entries));
        this.PageNumber = pageNumber;
        this.PageSize = pageSize;
        this.TotalItems = totalItems;
    }

    public IReadOnlyList<T> Entries { get; }

    public int PageNumber { get; }

    public long TotalItems { get; }

    public int PageSize { get; }

    public int TotalPages => (int)Math.Ceiling(this.TotalItems / (double)this.PageSize);

    public static Page<T> Empty()
    {
        return new Page<T>(Array.Empty<T>(), 1, 0, 0);
    }

    public Page<TOut> Convert<TOut>(Func<T, TOut> mappingFunction)
    {
        var mappedEntries = this.Entries
            .Select(mappingFunction)
            .ToList();

        return new Page<TOut>(mappedEntries, this.PageNumber, this.PageSize, this.TotalItems);
    }
}
