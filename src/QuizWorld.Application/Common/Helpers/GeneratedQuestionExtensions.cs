using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;
using QuizWorld.Application.Common.Models;

namespace QuizWorld.Application.Common.Helpers;

public static class GeneratedQuestionExtensions
{
    public static IEnumerable<Question> ToQuestions(this IEnumerable<GeneratedQuestion> generatedQuestions, Guid quizId, SkillTiny skill)
    {
        return generatedQuestions.Select(x => x.ToQuestion(quizId, skill.Id));
    }

    public static Question ToQuestion(this GeneratedQuestion generatedQuestion, Guid quizId, Guid skillId)
    {
        var answersCombinaisonsMapping = generatedQuestion.Answers.Where(a => a.Id is not null).ToDictionary(a => a.Id.GetValueOrDefault(), a => new Answer
        {
            Text = a.Text,
            Id = Guid.NewGuid()
        });

        var answers = generatedQuestion.Answers.Where(x => x.Id is null).Select(a => new Answer
        {
            Text = a.Text,
            IsCorrect = a.IsCorrect
        });

        var combinaisons = generatedQuestion.Combinaisons?.Select(c => c.Select(id => answersCombinaisonsMapping[id].Id).ToList()).ToList();

        var answersAndCombinaisons = answers.Concat(answersCombinaisonsMapping.Values).ToList();

        return new Question
        {
            Text = generatedQuestion.Text,
            Type = ConvertToQuestionType(generatedQuestion.Type),
            Answers = answersAndCombinaisons,
            Combinaisons = combinaisons,
            Status = Status.Pending,
            QuizId = quizId,
            SkillId = skillId
        };
    }

    private static QuestionType ConvertToQuestionType(string type)
    {
        return type.ToLower() switch
        {
            "simple" => QuestionType.SimpleChoice,
            "multiple" => QuestionType.MultipleChoice,
            "combinaison" => QuestionType.Combinaison,
            _ => QuestionType.Unknown
        };
    }
}
