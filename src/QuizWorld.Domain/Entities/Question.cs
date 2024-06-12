using QuizWorld.Domain.Common;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Domain.Entities;

/// <summary>
/// Represents a question entity.
/// </summary>
public class Question : BaseAuditableEntity
{
    /// <summary>Represents the text of the question.</summary>
    public string Text { get; set; } = default!;

    /// <summary>Represents the status of the question.</summary>
    public Status Status { get; set; } = Status.Pending;

    /// <summary>Represents the type of the question.</summary>
    public QuestionType Type { get; set; }

    /// <summary>Represents the answers of the question.</summary>
    public List<Answer> Answers { get; set; } = default!;

    /// <summary>Represents the combinaisons of answers. (if type was QuestionType.Combinaison)</summary>
    public List<List<Guid>>? Combinaisons { get; set; } = default!;

    /// <summary>Represents the quizId of the question.</summary>
    public Guid QuizId { get; set; }

    /// <summary>Represents the skill of the question.</summary>
    public SkillTiny Skill { get; set; } = default!;
}

public class QuestionTiny : BaseEntity
{
    /// <summary>Represents the text of the question.</summary>
    public string Text { get; set; } = default!;

    /// <summary>Represents the answers of the question.</summary>
    public List<AnswerTiny> Answers { get; set; } = default!;

    /// <summary>Represents the quizId of the question.</summary>
    public Guid QuizId { get; set; }

    /// <summary>Represents the skillId of the question.</summary>
    public Guid SkillId { get; set; }

    /// <summary>
    /// Represents the type of the question.
    /// </summary>
    public QuestionType? Type { get; set; } = null;
}

public class QuestionMinimal : BaseEntity
{
    /// <summary>Represents the text of the question.</summary>
    public string Text { get; set; } = default!;

    /// <summary>Represents the type of the question.</summary>
    public QuestionType Type { get; set; }
}

public static class QuestionExtensions
{
    public static QuestionTiny ToTiny(this Question question, bool displayType = false)
    {
        return new QuestionTiny
        {
            Id = question.Id,
            Text = question.Text,
            Answers = question.Answers.Select(a => a.ToTiny()).ToList(),
            QuizId = question.QuizId,
            SkillId = question.Skill.Id,
            Type = displayType ? question.Type : null
        };
    }

    public static QuestionMinimal ToMinimal(this Question question)
    {
        return new QuestionMinimal
        {
            Id = question.Id,
            Text = question.Text,
            Type = question.Type
        };
    }

    /// <summary>
    /// Check if the answer is correct.
    /// </summary>
    public static bool CheckAnswer(this Question question, List<Guid> answerIds)
    {
        if (question.Type == QuestionType.SimpleChoice || question.Type == QuestionType.MultipleChoice)
        {
            var correctAnswers = question.Answers?.Where(a => a.IsCorrect.HasValue && a.IsCorrect.Value).Select(a => a.Id).ToList();

            if (correctAnswers == null || correctAnswers.Count == 0)
                return false;

            if (correctAnswers.Count != answerIds.Count)
                return false;

            return correctAnswers.All(a => answerIds.Contains(a));
        }
        else if (question.Type == QuestionType.Combinaison)
        {
            if (question.Combinaisons == null || question.Combinaisons.Count == 0)
                return false;

            if (question.Combinaisons.Count != answerIds?.Count)
                return false;

            var userAnswersIdSet = new HashSet<Guid>(answerIds);

            foreach (var correctCombinaison in question.Combinaisons)
            {
                var correctCombinaisonSet = new HashSet<Guid>(correctCombinaison.Select(x => x));

                if (userAnswersIdSet.SetEquals(correctCombinaisonSet))
                    return true;
            }

            return false;
        }

        return false;
    }

    /// <summary>
    /// Extract the answers from the question.
    /// </summary>
    public static List<AnswerTiny> ExtractAnswers(this Question question, List<Guid> answerIds)
    {
        if (question.Type == QuestionType.SimpleChoice || question.Type == QuestionType.MultipleChoice)
        {
            return question.Answers?.Where(a => answerIds.Contains(a.Id)).Select(a => a.ToTiny()).ToList() ?? [];
        }
        else if (question.Type == QuestionType.Combinaison)
        {
            // For each answer in List<List<Answer>> Combinaisons, if the answer is in the answerIds list, add it to the answers list
            var answers = new List<AnswerTiny>();

            foreach (var combinaison in question.Combinaisons ?? [])
            {
                // TODO
                //answers.AddRange(combinaison.Where(a => answerIds.Contains(a));
            }

            return answers;
        }

        return [];
    }

    /// <summary>
    /// Check if the question has the answer.
    /// </summary>
    public static bool HasAnswer(this Question question, Guid answerId)
    {
        if (question.Type == QuestionType.SimpleChoice || question.Type == QuestionType.MultipleChoice)
        {
            return question.Answers?.Any(a => a.Id == answerId) ?? false;
        }
        else if (question.Type == QuestionType.Combinaison)
        {
            return question.Combinaisons?.Any(c => c.Any(a => a == answerId)) ?? false;
        }

        return false;
    }
}