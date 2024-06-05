﻿using AutoMapper;
using QuizWorld.Application.Common.Exceptions;
using QuizWorld.Application.Common.Helpers;
using QuizWorld.Application.Interfaces;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Application.MediatR.Questions.Commands.UpdateQuestion;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.Services;

public class QuestionService(IQuestionRepository questionRepository, IQuizService quizService, IUserSessionRepository userSessionRepository, IMapper mapper, IQuestionGenerator questionGenerator) : IQuestionService
{
    private readonly IQuestionRepository _questionRepository = questionRepository;
    private readonly IQuizService _quizService = quizService;
    private readonly IUserSessionRepository _userSessionRepository = userSessionRepository;
    private readonly IQuestionGenerator _questionGenerator = questionGenerator;
    private readonly IMapper _mapper = mapper;

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
    public async Task<List<QuestionTiny>> GetCustomQuestions(Guid quizId, Guid userId)
    {
        var quiz = await _quizService.GetByIdAsync(quizId)
            ?? throw new NotFoundException(nameof(Quiz), quizId);

        var questions = await _questionRepository.GetQuestionsByQuizIdAsync(quizId);

        if (quiz.PersonalizedQuestions)
        {
            // TODO: Add algorithm to get custom questions adapted to the user
        }

        return questions.Take(quiz.TotalQuestions).Select(q => q.ToTiny()).ToList();
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
    public async Task<bool> UpdateQuestion(UpdateQuestionCommand request)
    {
        var question = await GetQuestionById(request.QuestionId)
            ?? throw new NotFoundException(nameof(Question), request.QuestionId);

        var result = false;

        if (question.QuizId == request.QuizId)
        {
            var newQuestion = request.Question.ToQuestion(question.QuizId, question.SkillId);
            newQuestion.Id = question.Id;
            newQuestion.CreatedAt = question.CreatedAt;
            
            result = await _questionRepository.UpdateQuestionAsync(request.QuestionId, newQuestion);
        }

        return result;
    }

    /// <inheritdoc/>
    public async Task<Question> ValidateQuestion(Guid quizId, Guid questionId, bool isValid)
    {
        var question = await _questionRepository.GetByIdAsync(questionId)
            ?? throw new NotFoundException(nameof(Question), questionId);

        if (question.QuizId != quizId)
            throw new NotFoundException(nameof(Question), questionId);

        question.Status = isValid ? Status.Valid : Status.Invalid;

        await _questionRepository.UpdateStatus(question.Id, question.Status);

        return question;
    }
}
