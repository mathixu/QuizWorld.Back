using QuizWorld.Application.Common.Exceptions;
using QuizWorld.Application.Common.Helpers;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Application.MediatR.Questions.Commands.AnswerQuestion;
using QuizWorld.Application.MediatR.Questions.Commands.UpdateQuestion;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.Services;

public class QuestionService(IQuestionRepository questionRepository,
    IQuizService quizService, 
    IQuestionStatsRepository questionStatsRepository, 
    IUserSessionRepository userSessionRepository, 
    IQuestionGenerator questionGenerator,
    IUserResponseRepository userResponseRepository,
    ICurrentSessionService currentSessionService
    ) : IQuestionService
{
    private readonly IQuestionRepository _questionRepository = questionRepository;
    private readonly IQuizService _quizService = quizService;
    private readonly IUserSessionRepository _userSessionRepository = userSessionRepository;
    private readonly ICurrentSessionService _currentSessionService = currentSessionService;
    private readonly IQuestionGenerator _questionGenerator = questionGenerator;
    private readonly IQuestionStatsRepository _questionStatsRepository = questionStatsRepository;
    private readonly IUserResponseRepository _userResponseRepository = userResponseRepository;

    /// <inheritdoc/>
    public async Task CreateQuestionsAsync(Quiz quiz)
    {
        List<Question> questions = [];

        foreach (var skillWeight in quiz.SkillWeights)
        {
            var skill = skillWeight.Skill;

            var skillTotalQuestions = GetSkillTotalQuestions(quiz.TotalQuestions, skillWeight.Weight);

            var questionsGenerated = await _questionGenerator.GenerateQuestionsBySkills(quiz.Id, skill, skillTotalQuestions, quiz.Attachment);

            questions.AddRange(questionsGenerated);
        }

        await _questionRepository.AddRangeAsync(questions);
    }
    
    /// <inheritdoc/>
    public async Task<Question> RegenerateQuestion(Guid quizId, Guid questionId, string requirement)
    {
        var question = await _questionRepository.GetByIdAsync(questionId) 
            ?? throw new NotFoundException("Question not found");

        var quiz = await _quizService.GetByIdAsync(quizId) 
            ?? throw new NotFoundException("Quiz not found");

        if (question.QuizId != quiz.Id)
        {
            throw new BadRequestException("The quizId doesn't match with the questionId.");
        }

        var regeneratedQuestion = await _questionGenerator.RegenerateQuestion(question.Skill, question, requirement, quiz.Attachment);
        regeneratedQuestion.Id = questionId;

        await _questionRepository.UpdateQuestionAsync(questionId, regeneratedQuestion);

        return regeneratedQuestion;
    }

    /// <inheritdoc/>
    public async Task<List<QuestionTiny>> GetCustomQuestions(Guid quizId, Guid userId)
    {
        var quiz = await _quizService.GetByIdAsync(quizId)
            ?? throw new NotFoundException(nameof(Quiz), quizId);
        
        var questions = await _questionRepository.GetQuestionsByQuizIdAsync(quizId);

        var currentUserSession = _currentSessionService.GetUserSessionByUserId(userId)
            ?? throw new UnauthorizedAccessException("The user is not connected to a session.");

        var userSession = await _userSessionRepository.GetByIdAsync(currentUserSession.Id)
            ?? throw new UnauthorizedAccessException("The user is not connected to a session.");

        var questionSelected = new List<QuestionTiny>();
        if (userSession.QuestionIds.Count != 0)
        {
            if (userSession.QuestionIds.All(q => questions.Any(x => x.Id == q)))
            {
                questionSelected = userSession.QuestionIds.Select(q => questions.First(x => x.Id == q).ToTiny(quiz.DisplayMultipleChoice)).ToList();

                questionSelected.ForEach(q => q.Answers = q.Answers.Shuffle().ToList());

                return questionSelected;
            }

            throw new NotFoundException("One or more questions in the session are not found.");
        }

        if (quiz.PersonalizedQuestions)
        {
            var userResponses = await _userResponseRepository.GetUserQuizResponses(userId, quizId);
            
            Dictionary<Question, double> questionWeights = new();

            var questionsOld = questions.OrderBy(x => userResponses.FirstOrDefault(y => y.Question.Id == x.Id)?.UpdatedAt ?? DateTime.MinValue).ToList();
            
            Random random = new Random();

            foreach (Question question in questions)
            {
                var userResponse = userResponses.FirstOrDefault(x => x.Question.Id == question.Id);
                var weight = 0.0;

                if (userResponse is null)
                {
                    double randomNumber = (random.NextDouble() % 0.4) + 0.8;
                    weight += 100 * randomNumber;
                }
                if (questionsOld.IndexOf(question) < questionsOld.Count / 10)
                {
                    double randomNumber = (random.NextDouble() % 0.4) + 0.8;

                    weight += 60 * randomNumber;
                }
                    
                if (userResponse is not null && userResponse.SuccessRate < (question.Skill.MasteryThreshold / 100))
                {
                    double randomNumber = (random.NextDouble() % 0.4) + 0.8;

                    weight += 40 * randomNumber;
                }
                
                if (userResponse is not null && !userResponse.LastResponseIsCorrect)
                {
                    double randomNumber = (random.NextDouble() % 0.4) + 0.8;
                    weight += 35 * randomNumber;
                }
                
                questionWeights.Add(question, weight);
            }
            questionSelected = questionWeights.OrderByDescending(x => x.Value).Take(quiz.TotalQuestions).Select(x => x.Key.ToTiny(quiz.DisplayMultipleChoice)).ToList();
        }
        else
        {
            questionSelected = questions.OrderBy(x => Guid.NewGuid()).Take(quiz.TotalQuestions).Select(q => q.ToTiny(quiz.DisplayMultipleChoice)).ToList();
        }

        questionSelected.ForEach(q => q.Answers = q.Answers.Shuffle().ToList());

        userSession.QuestionIds = questionSelected.Select(q => q.Id).ToList();
        userSession.Result = new UserSessionResult
        {
            StartedAt = DateTime.UtcNow,
            TotalQuestions = questionSelected.Count,
            QuestionsAnswered = 0
        };

        await _userSessionRepository.UpdateAsync(userSession.Id, userSession);

        return questionSelected;
    }

    /// <inheritdoc/>
    public async Task<bool> AnswerQuestionAsync(Guid questionId, List<Guid> answerIds)
    {
        var question = await _questionRepository.GetByIdAsync(questionId)
            ?? throw new NotFoundException(nameof(Question), questionId);

        return question.CheckAnswer(answerIds);
    }

    /// <inheritdoc/>
    public async Task<Question?> GetQuestionById(Guid questionId)
    {
        return await _questionRepository.GetByIdAsync(questionId);
    }

    private static int GetSkillTotalQuestions(int quizTotalQuestion, int weight)
    {
        var minimumQuestions = quizTotalQuestion * weight / 100 * 2;

        return minimumQuestions < 1 ? 1 : minimumQuestions;
    }

    /// <inheritdoc/>
    public async Task<Question> UpdateQuestion(UpdateQuestionCommand request)
    {
        var question = await GetQuestionById(request.QuestionId)
            ?? throw new NotFoundException(nameof(Question), request.QuestionId);

        if (question.QuizId != request.QuizId)
        {
            throw new BadRequestException("The quizId of the question does not match with the quizId.");
        }

        var newQuestion = request.Question.ToQuestion(question.QuizId, question.Skill);

        newQuestion.Id = question.Id;
        newQuestion.CreatedAt = question.CreatedAt;

        await _questionRepository.UpdateQuestionAsync(request.QuestionId, newQuestion);

        return newQuestion;
    }

    /// <inheritdoc/>
    public async Task<Question> UpdateQuestionStatus(Guid quizId, Guid questionId, Status status)
    {
        var question = await GetQuestionById(questionId)
            ?? throw new NotFoundException(nameof(Question), questionId);

        if (question.QuizId != quizId)
        {
            throw new BadRequestException("The quizId of the question does not match with the quizId.");
        }

        question.Status = status;

        await _questionRepository.UpdateStatus(question.Id, status);

        return question;
    }
    
    /// <inheritdoc/>
    public async Task<WebSocketAction> ProcessUserResponse(UserSession userSession, AnswerQuestionCommand command)
    {
        var question = await GetQuestionById(command.QuestionId)
            ?? throw new NotFoundException(nameof(Question), command.QuestionId);

        var questionMinimal = question.ToMinimal();

        if (question.QuizId != command.QuizId)
            throw new BadRequestException("The question does not belong to the quiz.");

        if (!command.AnswerIds.All(x => question.HasAnswer(x)))
            throw new BadRequestException("One or more answers do not belong to the question.");

        var responseIsCorrect = question.CheckAnswer(command.AnswerIds);

        await UpdateUserResponse(userSession.User, command.QuizId, question, responseIsCorrect);

        var result = await UpdateUserSession(userSession, question, responseIsCorrect);

        await UpdateQuestionStats(questionMinimal, responseIsCorrect);

        if (result.QuestionsAnswered == result.TotalQuestions)
            return WebSocketAction.UserFinishedQuiz;

        return WebSocketAction.None;
    }

    private async Task<UserSessionResult> UpdateUserSession(UserSession userSession, Question question, bool isCorrect)
    {
        if (userSession.Result is null)
        {
            throw new BadRequestException("The user session result is not initialized.");
        }

        userSession.Result.QuestionsAnswered++;
        userSession.Result.Note = (userSession.Result.Note ?? 0) + (isCorrect ? 1 : 0);

        userSession.Result.SkillWeights ??= [];

        var skillWeight = userSession.Result.SkillWeights.FirstOrDefault(x => x.Skill.Id == question.Skill.Id);

        if (skillWeight is null)
        {
            skillWeight = new SkillWeightExtended
            {
                Skill = question.Skill,
                Attempts = 1,
                Weight = isCorrect ? 100 : 0
            };

            userSession.Result.SkillWeights.Add(skillWeight);
        }
        else
        {
            UpdateSkillWeight(skillWeight, isCorrect);
        }

        if (userSession.Result.QuestionsAnswered == userSession.Result.TotalQuestions)
        {
            userSession.Result.EndedAt = DateTime.UtcNow;
        }

        await _userSessionRepository.UpdateAsync(userSession.Id, userSession);

        return userSession.Result;
    }

    private static void UpdateSkillWeight(SkillWeightExtended skillWeight, bool isCorrect) 
    {
        var currentSuccessRate = skillWeight.Weight / 100.0;
        var totalAttempts = skillWeight.Attempts;

        var previousSuccesses = Math.Round(currentSuccessRate * totalAttempts);
            
        var newSuccesses = isCorrect ? previousSuccesses + 1 : previousSuccesses;

        skillWeight.Attempts++;

        skillWeight.Weight = (int)Math.Round(newSuccesses / skillWeight.Attempts * 100);
    }

    private async Task UpdateUserResponse(UserTiny user, Guid quizId, Question question, bool isCorrect)
    {
        var userResponse = await _userResponseRepository.GetUserResponse(user.Id, quizId, question.Id);

        if (userResponse is not null)
        {
            userResponse.SuccessRate = (userResponse.SuccessRate * userResponse.Attempts + (isCorrect ? 1 : 0)) / (userResponse.Attempts + 1);
            userResponse.Attempts++;
            userResponse.LastResponseIsCorrect = isCorrect;

            await _userResponseRepository.UpdateAsync(userResponse.Id, userResponse);

            return;
        }

        var response = new UserResponse
        {
            User = user,
            QuizId = quizId,
            SkillId = question.Skill.Id,
            Question = question.ToMinimal(),
            Attempts = 1,
            SuccessRate = isCorrect ? 1 : 0,
            LastResponseIsCorrect = isCorrect,
        };

        await _userResponseRepository.AddAsync(response);
    }

    private async Task UpdateQuestionStats(QuestionMinimal question, bool isCorrect)
    {
        var questionStats = await _questionStatsRepository.GetByQuestionIdAsync(question.Id);

        if (questionStats is not null)
        {
            questionStats.SuccessRate = (questionStats.SuccessRate * questionStats.Attempts + (isCorrect ? 1 : 0)) / (questionStats.Attempts + 1);
            questionStats.Attempts++;

            await _questionStatsRepository.UpdateAsync(question.Id, questionStats);

            return;
        } 

        questionStats = new QuestionStats
        {
            Attempts = 1,
            Question = question,
            SuccessRate = isCorrect ? 1 : 0,
        };

        await _questionStatsRepository.AddAsync(questionStats);
    }
}
