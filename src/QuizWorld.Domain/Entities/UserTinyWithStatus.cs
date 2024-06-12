namespace QuizWorld.Domain.Entities;

public class UserTinyWithStatus : UserTiny
{
    public UserStatus Status { get; set; }
}

public enum UserStatus
{
    Waiting = 0,
    InProgress = 1,
    Done = 2
}
