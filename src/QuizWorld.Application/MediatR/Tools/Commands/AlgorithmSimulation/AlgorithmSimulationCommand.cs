using QuizWorld.Application.MediatR.Common;
using QuizWorld.Domain.Entities;
using System.Text;

namespace QuizWorld.Application.MediatR.Tools.Commands.AlgorithmSimulation;

public record AlgorithmSimulationCommand(Guid QuizId, int Turn) : IQuizWorldRequest<AlgorithmSimulationResponse>;

public class AlgorithmSimulationResponse
{
    public string Lisible => FormatAlgorithmSimulation();

    public Guid QuizId { get; set; }
    public int Turn { get; set; }

    public List<UserResponse> UserResponses { get; set; } = [];

    public List<KeyValuePair<QuestionMinimal, double>> Questions { get; set; } = [];


    private string FormatAlgorithmSimulation()
    {
        /*
        QuizId: QuizId
        Turn: Turn
        Questions:
            - UserResponses.Question.Text: (UserResponses.SuccessRate*100)% de succès
            - UserResponses.Question.Text: (UserResponses.SuccessRate*100)% de succès
            - UserResponses.Question.Text: (UserResponses.SuccessRate*100)% de succès

        SelectedQuestions:
            - Questions.Key.Text: Questions.Value (weight)
            - Questions.Key.Text: Questions.Value (weight)
            - Questions.Key.Text: Questions.Value (weight)
         */

        var sb = new StringBuilder();

        sb.AppendLine($"QuizId: {QuizId}");
        sb.AppendLine($"Turn: {Turn}");
        sb.AppendLine("Questions: (success rate: question)");
        sb.AppendLine(
            string.Join(
                Environment.NewLine,
                UserResponses.Select(
                    x => $"- {x.SuccessRate * 100}: {x.Question.Text}"
                )));
        sb.AppendLine();
        sb.AppendLine("SelectedQuestions: (weight: question)");
        sb.AppendLine(
                string.Join(
                Environment.NewLine,
                Questions.Select(
                x => $"- {x.Value}: {x.Key.Text}"
            )));

        return sb.ToString();
    }
}
