﻿using Microsoft.AspNetCore.Http;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.MediatR.Quizzes.Commands.CreateQuiz;
using QuizWorld.Application.MediatR.Quizzes.Commands.StartQuiz;
using QuizWorld.Application.MediatR.Quizzes.Queries.SearchQuizzes;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.Interfaces;

/// <summary>
/// Represents a service for quiz operations.
/// </summary>
public interface IQuizService
{
    /// <summary>Creates a quiz</summary>
    Task<Quiz> CreateQuizAsync(CreateQuizCommand command);

    /// <summary>Searches quizzes</summary>
    Task<PaginatedList<QuizTiny>> SearchQuizzesAsync(SearchQuizzesQuery query);

    /// <summary>Gets a quiz by its id</summary>
    Task<Quiz?> GetByIdAsync(Guid id);

    /// <summary>Adds an attachment to a quiz</summary>
    Task<bool> AddAttachmentToQuiz(Guid quizId, IFormFile attachment);

    /// <summary>Gets quizzes by their ids</summary>
    Task<List<Quiz>> GetQuizzesByIds(List<Guid> ids);

    /// <summary>Changes the status of a quiz.</summary>
    /// <param name="quizId">The quiz id.</param>
    /// <param name="status">The new status.</param>
    Task<bool> UpdateQuizStatus(Guid quizId, QuizStatus status);


    Task<Quiz> ValidateQuizAsync(Guid quizId);
}
