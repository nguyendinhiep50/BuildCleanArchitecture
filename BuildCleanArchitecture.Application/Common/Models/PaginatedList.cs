using Microsoft.EntityFrameworkCore;

namespace BuildCleanArchitecture.Application.Common.Models;

public class PaginatedList<T>
{
    public IReadOnlyCollection<T> Items { get; }
    public int PageNumber { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }
    public bool HasMoreData { get; }

    public PaginatedList(IReadOnlyCollection<T> items, int count, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
        Items = items;
        HasMoreData = pageNumber * pageSize < count;
    }

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }

    //public static PaginatedList<T> Create(IList<T> source, PagingResponseSP pagingResponseSP)
    //{
    //    var count = pagingResponseSP.TotalRows;
    //    var items = source;

    //    return new PaginatedList<T>(items.ToList(), count, pagingResponseSP.CurrentPage, pagingResponseSP.PageSize);
    //}

}
