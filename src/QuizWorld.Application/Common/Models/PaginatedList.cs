using AutoMapper;

namespace QuizWorld.Application.Common.Models;

/// <summary>Represents generic paginated list.</summary>
/// <typeparam name="T">The type of the items.</typeparam>
public class PaginatedList<T>
{
    /// <summary>Represents the page index.</summary>
    public int PageIndex { get; set; }

    /// <summary>Represents the total number of pages.</summary>
    public int TotalPages { get; set; }

    /// <summary>Represents the page size.</summary>
    public int PageSize { get; set; }

    /// <summary>Represents the total count of items.</summary>
    public long TotalCount { get; set; }

    /// <summary>Represents whether there is a previous page.</summary>
    public bool HasPreviousPage => PageIndex > 1;

    /// <summary>Represents whether there is a next page.</summary>
    public bool HasNextPage => PageIndex < TotalPages;

    /// <summary>
    /// Represents the items.
    /// </summary>
    public IEnumerable<T> Items { get; set; }

    public PaginatedList(IEnumerable<T> items, long count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        PageSize = pageSize;
        TotalCount = count;
        Items = items;
    }

    /// <summary>
    /// Maps the items to the destination type.
    /// </summary>
    /// <typeparam name="TDestination">The destination type.</typeparam>
    /// <param name="mapper">The mapper instance to use for mapping.</param>
    /// <returns>The paginated list with the items mapped to the destination type.</returns>
    public PaginatedList<TDestination> Map<TDestination>(IMapper mapper)
    {
        var items = mapper.Map<IEnumerable<TDestination>>(Items);
        return new PaginatedList<TDestination>(items, TotalCount, PageIndex, PageSize);
    }

    /// <summary>
    /// Maps the items to the destination type using a conversion function.
    /// </summary>
    /// <typeparam name="TDestination">The destination type.</typeparam>
    /// <param name="conversion">The conversion function to use for each item.</param>
    /// <returns>The paginated list with the items mapped to the destination type.</returns>
    public PaginatedList<TDestination> Map<TDestination>(Func<T, TDestination> conversion)
    {
        var items = Items.Select(item => conversion(item));
        return new PaginatedList<TDestination>(items, TotalCount, PageIndex, PageSize);
    }
}
