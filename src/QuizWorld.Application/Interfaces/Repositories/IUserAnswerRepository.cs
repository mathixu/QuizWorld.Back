using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.Interfaces.Repositories
{
    public interface IUserAnswerRepository
    {
        Task<bool> AddAsync(UserAnswer userAnswer);
        Task<List<UserAnswer>> GetUserAnswers(Guid sessionId, Guid userId);
    }
}
