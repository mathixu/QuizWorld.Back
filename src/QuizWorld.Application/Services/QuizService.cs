using AutoMapper;
using QuizWorld.Application.Common.Exceptions;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Application.MediatR.Quizzes.Commands.CreateQuiz;
using QuizWorld.Application.MediatR.Quizzes.Queries.SearchQuizzes;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.Services;

/// <summary>Represents a service for quiz operations.</summary>
public class QuizService(IQuizRepository quizRepository, 
    ISkillRepository skillRepository, 
    ICurrentUserService currentUserService, 
    IMapper mapper) : IQuizService
{
    private readonly IQuizRepository _quizRepository = quizRepository;
    private readonly ISkillRepository _skillRepository = skillRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    /// <inheritdoc/>
    public async Task<Quiz> CreateQuizAsync(CreateQuizCommand command)
    {
        var quiz = _mapper.Map<Quiz>(command);

        quiz.SkillWeights = await BuildSkillWeights(command.SkillWeights);

        // TODO: Add exception if user is null
        quiz.CreatedBy = _currentUserService.User ?? new UserTiny() { Id = Guid.Empty, Email = "default@email.com"}; 

        await _quizRepository.AddAsync(quiz);

        return quiz;
    }

    public async Task<PaginatedList<QuizTiny>> SearchQuizzesAsync(SearchQuizzesQuery query)
    {
        var quizzes = await _quizRepository.SearchQuizzesAsync(query);

        var quizzesTiny = quizzes.Map(x => x.ToTiny());

        return quizzesTiny;
    }

    private async Task<List<SkillWeight>> BuildSkillWeights(Dictionary<Guid, int> skillWeights)
    { 
        var skills = await _skillRepository.GetSkillsByIdsAsync(skillWeights.Select(x => x.Key));

        if (skills.Count != skillWeights.Count)
        {
            throw new BadRequestException("One or more skills were not found.");
        }

        return skillWeights.Select(x => new SkillWeight
            { Skill = skills.First(s => s.Id == x.Key).ToTiny(), Weight = x.Value })
            .ToList();  
    }
}
