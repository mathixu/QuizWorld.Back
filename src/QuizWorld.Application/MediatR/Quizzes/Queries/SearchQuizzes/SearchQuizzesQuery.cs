﻿using QuizWorld.Application.Common.Models;
using QuizWorld.Application.MediatR.Common;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Quizzes.Queries.SearchQuizzes;

public class SearchQuizzesQuery : PaginationQuery, IQuizWorldRequest<PaginatedList<QuizTiny>>
{
    /// <summary>
    /// Represents the name of the quiz.
    /// </summary>
    public string? Name { get; set; } = default!;
}