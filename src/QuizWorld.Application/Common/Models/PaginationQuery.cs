namespace QuizWorld.Application.Common.Models;

/// <summary>
/// Represents the query for pagination.
/// </summary>
public class PaginationQuery
{
    /// <summary>
    /// Represents the page number.
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Represents the page size.
    /// </summary>
    public int PageSize { get; set; } = 25;
}
