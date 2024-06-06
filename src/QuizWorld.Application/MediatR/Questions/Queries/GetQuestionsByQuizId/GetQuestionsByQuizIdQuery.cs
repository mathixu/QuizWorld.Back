using QuizWorld.Application.Common.Models;
using QuizWorld.Application.MediatR.Common;
using QuizWorld.Domain.Entities;
using System.Text.Json.Serialization;

namespace QuizWorld.Application.MediatR.Questions.Queries.GetQuestionsByQuizId;

/// <summary>The query to get the questions by the quiz id.</summary>
public class GetQuestionsByQuizIdQuery(Guid quizId, int Page, int PageSize) : PaginationQuery(Page, PageSize), IQuizWorldRequest<PaginatedList<Question>> 
{
    /// <summary>The id of the quiz.</summary>
    [JsonIgnore]
    public Guid QuizId { get; set; } = quizId;
}