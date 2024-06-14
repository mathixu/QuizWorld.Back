using QuizWorld.Application.Common.Models;
using QuizWorld.Application.MediatR.Questions.Commands.AnswerQuestion;
using QuizWorld.Application.MediatR.Questions.Commands.UpdateQuestion;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.Interfaces;

/// <summary>
/// Represents the service for the questions.
/// </summary>
public interface IQuestionService
{
    /// <summary>
    /// Creates the questions for the quiz.
    /// </summary>
    Task CreateQuestionsAsync(Quiz quiz);

    /// <summary>
    /// Gets the custom questions for a User.
    /// </summary>
    Task<List<QuestionTiny>> GetCustomQuestions(Guid quizId, Guid userId);

    /// <summary>
    /// Answers a question.
    /// </summary>
    Task<bool> AnswerQuestionAsync(Guid questionId, List<Guid> answerIds);

    /// <summary>
    /// Gets a question by its id.
    /// </summary>
    Task<Question?> GetQuestionById(Guid questionId);

    /// <summary>
    /// Edit a question.
    /// </summary>
    Task<Question> UpdateQuestion(UpdateQuestionCommand question);

    /// <summary>
    /// Updates the status of a question.
    /// </summary>
    Task<Question> UpdateQuestionStatus(Guid quizId, Guid questionId, Status status);

    /// <summary>
    /// Processes the user response.
    /// </summary>
    Task ProcessUserResponse(UserSession userSession, AnswerQuestionCommand command);

    /// <summary>
    /// Regenerate a new question with IA.
    /// </summary>
    /// <param name="quizId">the id of the quiz.</param>
    /// <param name="questionId">the id of the question.</param>
    /// <param name="requirement">the requirement for the regeneration.</param>
    /// <returns>the new question.</returns>
    Task<Question> RegenerateQuestion(Guid quizId, Guid questionId, string requirement);
}
