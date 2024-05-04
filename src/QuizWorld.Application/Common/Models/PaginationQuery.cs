namespace QuizWorld.Application.Common.Models;

/// <summary>
/// Represents the query for pagination.
/// </summary>
public class PaginationQuery
{
    /// <summary>
    /// Represents the page number.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Represents the page size.
    /// </summary>
    public int PageSize { get; set; }

    public PaginationQuery(int page = 1, int pageSize = 25)
    {
        Page = page;
        PageSize = pageSize;
    }
}
