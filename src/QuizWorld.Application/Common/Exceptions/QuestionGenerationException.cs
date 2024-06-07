namespace QuizWorld.Application.Common.Exceptions;

public class QuestionGenerationException : Exception
{
    public QuestionGenerationException() : base()
    {
    }

    public QuestionGenerationException(string message) : base(message)
    {
    }
}
