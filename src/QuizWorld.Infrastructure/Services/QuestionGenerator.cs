using Microsoft.Extensions.Options;
using QuizWorld.Application.Common.Exceptions;
using QuizWorld.Application.Common.Helpers;
using QuizWorld.Application.Interfaces;
using QuizWorld.Domain.Entities;
using QuizWorld.Infrastructure.Common.Models;
using QuizWorld.Infrastructure.Common.Options;
using QuizWorld.Infrastructure.Interfaces;
using System.Text.Json;
using QuizWorld.Application.Common.Models;

namespace QuizWorld.Infrastructure.Services;

public class QuestionGenerator(
    ILLMService LLMService, 
    IGeneratedContentRepository generatedContentRepository, 
    IOptions<LLMOptions> _options) : IQuestionGenerator
{
    private readonly ILLMService _LLMService = LLMService;
    private readonly IGeneratedContentRepository _generatedContentRepository = generatedContentRepository;
    private readonly LLMOptions _options = _options.Value;

    /// <inheritdoc />
    public async Task<List<Question>> GenerateQuestionsBySkills(Guid quizId, SkillTiny skill, int totalQuestions, QuizFile? file)
    {
        int maxAttempts = _options.MaxGenerationAttempts;
        int attempt = 0; 
        string contentGenerated = string.Empty;

        while (attempt < maxAttempts)
        {
            try
            {
                var input = BuildInput(skill.Name, totalQuestions);

                contentGenerated = await _LLMService.GenerateContent(GenerateContentType.QuestionsBySkills, input, file?.FileName);

                var generatedQuestions = DeserializeQuestions(contentGenerated);

                await SaveAsync(skill.Id, quizId, input, contentGenerated, GenerateContentType.QuestionsBySkills, generatedQuestions is null, attempt);

                return generatedQuestions?.ToQuestions(quizId, skill).ToList() ?? [];
            }
            catch (Exception ex)
            {
                attempt++;

                await Task.Delay(15000);

                if (attempt >= maxAttempts) 
                {
                    var objectResponse = new
                    {
                        Message = "Error generating questions",
                        Ex = ex.Message,
                        ContentGenerated = contentGenerated,
                        Attempt = attempt
                    };

                    throw new QuestionGenerationException(JsonSerializer.Serialize(objectResponse));
                }
            }
        }

        throw new QuestionGenerationException("Error generating questions");
    }    

    public async Task<Question> RegenerateQuestion(Skill skill, Question question, string requirement, QuizFile? attachment)
    {
        int maxAttempts = _options.MaxGenerationAttempts;
        int attempt = 0;
        string contentGenerated = string.Empty;

        while (attempt < maxAttempts)
        {
            try
            {
                var input = RegenerateBuildInput(skill.NameNormalized, question, requirement);

                contentGenerated = await _LLMService.GenerateContent(GenerateContentType.RegenerateQuestion, input, attachment?.FileName);

                var generatedQuestions = DeserializeQuestions(contentGenerated);

                if (generatedQuestions == null || generatedQuestions.Count == 0)
                {
                    throw new QuestionGenerationException("No question regenerated");
                }

                var regeneratedQuestion = generatedQuestions.First().ToQuestion(question.QuizId, skill.Id);

                await SaveRegenerateAsync(skill.Id, question.QuizId, input, contentGenerated, GenerateContentType.RegenerateQuestion, false, attempt);

                return regeneratedQuestion;
            }
            catch (Exception ex)
            {
                attempt++;

                await Task.Delay(15000);

                if (attempt >= maxAttempts)
                {
                    var objectResponse = new
                    {
                        Message = "Error regenerating question",
                        Ex = ex.Message,
                        ContentGenerated = contentGenerated,
                        Attempt = attempt
                    };

                    throw new QuestionGenerationException(JsonSerializer.Serialize(objectResponse));
                }
            }
        }

        throw new QuestionGenerationException("Error regenerating question");
    }

    private static List<GeneratedQuestion>? DeserializeQuestions(string contentGenerated)
    {
        try
        {
            return JsonSerializer.Deserialize<List<GeneratedQuestion>>(contentGenerated.FormatFromLLM());
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private static string BuildInput(string name, int totalQuestions)
    {
        var payload = new
        {
            skill = name,
            number = totalQuestions,
        };

        return JsonSerializer.Serialize(payload);
    }

    private static string RegenerateBuildInput(string skillName, Question question, string requirement)
    {
        var payload = new
        {
            skill = skillName,
            requirement,
            text = question.Text,
            answers = question.Answers.Select(a => new { a.Text, a.IsCorrect }),
            type = question.Type
        };

        return JsonSerializer.Serialize(payload);
    }

    private async Task<bool> SaveAsync(Guid skillId, Guid quizId, string input, string contentGenerated, GenerateContentType contentType, bool hasError, int attempt)
    {
        var generatedContent = new GeneratedContent
        {
            SkillId = skillId,
            QuizId = quizId,
            Input = input,
            Content = contentGenerated,
            ContentFormatted = contentGenerated.FormatFromLLM(),
            ContentType = contentType,
            HasError = hasError,
            Model = _options.Model,
            Attempt = attempt
        };

        return await _generatedContentRepository.AddAsync(generatedContent);
    }


    private async Task<bool> SaveRegenerateAsync(Guid skillId, Guid quizId, string input, string contentGenerated, GenerateContentType contentType, bool hasError, int attempt)
    {
        var generatedContent = new GeneratedContent
        {
            SkillId = skillId,
            QuizId = quizId,
            Input = input,
            Content = contentGenerated,
            ContentFormatted = contentGenerated.FormatFromLLM(),
            ContentType = contentType,
            HasError = hasError,
            Model = _options.Model,
            Attempt = attempt
        };

        return await _generatedContentRepository.AddAsync(generatedContent);
    }
}