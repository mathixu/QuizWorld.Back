using QuizWorld.Domain.Common;

namespace QuizWorld.Domain.Entities
{
    public class UserAnswer : BaseEntity
    {
        public Guid SessionId { get; set; }

        public Guid QuizId { get; set; }

        public Guid UserId { get; set; }

        public Guid QuestionId { get; set; }

        public List<Guid> AnswerIds { get; set; }

        public bool IsCorrect { get; set; }
    }
}
