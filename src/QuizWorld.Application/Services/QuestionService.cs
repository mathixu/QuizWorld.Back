using QuizWorld.Application.Interfaces;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.Services;

public class QuestionService(IQuestionRepository questionRepository) : IQuestionService
{
    private readonly IQuestionRepository _questionRepository = questionRepository;

    // Fake implementation
    /// <inheritdoc/>
    public async Task CreateQuestionsAsync(Quiz quiz)
    {
        var questions = new List<Question>();

        for(int i = 0; i < quiz.TotalQuestions; i++)
        {
            var question = new Question
            {
                Text = $"How much is {i} + {i * 2}?",
                Type = QuestionType.SimpleChoice,
                Answers = [
                    new Answer { Text = $"{i + i * 2}", IsCorrect = true },
                    new Answer { Text = $"{i + i * 2 + 1}", IsCorrect = false },
                    new Answer { Text = $"{i + i * 2 + 2}", IsCorrect = false },
                    new Answer { Text = $"{i + i * 2 + 3}", IsCorrect = false },
                    new Answer { Text = $"{i + i * 2 + 4}", IsCorrect = false },
                ],
                QuizId = quiz.Id,
            };

            for(int j = 0; j < i%3; j++)
            {
                question.Answers.Add(new Answer { Text = $"{i + i * 2 + j + 5}", IsCorrect = false });
            }

            questions.Add(question);
        }

        await _questionRepository.AddRangeAsync(questions);
    }
}
