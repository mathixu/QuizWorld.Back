using AutoMapper;
using Microsoft.AspNetCore.Http;
using QuizWorld.Application.Common.Exceptions;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Application.MediatR.Quizzes.Commands.CreateQuiz;
using QuizWorld.Application.MediatR.Quizzes.Queries.SearchQuizzes;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.Services;

/// <summary>Represents a service for quiz operations.</summary>
public class QuizService(IQuizRepository quizRepository, 
    ISkillRepository skillRepository, 
    ICurrentUserService currentUserService, 
    IMapper mapper,
    IStorageService storageService
    ) : IQuizService
{
    private readonly IQuizRepository _quizRepository = quizRepository;
    private readonly ISkillRepository _skillRepository = skillRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IStorageService _storageService = storageService;

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

    /// <inheritdoc/>
    public async Task<bool> AddAttachmentToQuiz(Guid quizId, IFormFile attachment)
    {
        var quiz = await _quizRepository.GetByIdAsync(quizId) 
            ?? throw new NotFoundException(nameof(Quiz), quizId);

        if (quiz.Attachment is null)
            throw new BadRequestException("This quiz does not allow attachments.");

        if (quiz.Attachment.Status == QuizFileStatus.Uploaded)
            throw new BadRequestException("This quiz already has an attachment.");

        // TODO: Add exception if user is null
        var currentUser = _currentUserService.User ?? new UserTiny() { Id = Guid.Empty, Email = "default@email.com" };

        if (quiz.CreatedBy.Id != currentUser.Id)
            throw new ForbiddenAccessException("You are not allowed to add attachments to this quiz.");

        var fileUrl = await _storageService.UploadFileAsync(attachment);

        if (string.IsNullOrEmpty(fileUrl))
        {
            var failedAttachment = new QuizFile
            {
                Status = QuizFileStatus.Failed,
                UploadedAt = DateTime.UtcNow
            };

            await _quizRepository.UpdateAttachmentToQuizAsync(quizId, failedAttachment);

            return false;
        }

        var newAttachment = BuildAttachment(attachment);

        newAttachment.Url = fileUrl;

        await _quizRepository.UpdateAttachmentToQuizAsync(quizId, newAttachment);

        return true;
    }
    
    /// <inheritdoc/>
    public async Task<PaginatedList<QuizTiny>> SearchQuizzesAsync(SearchQuizzesQuery query)
    {
        var quizzes = await _quizRepository.SearchQuizzesAsync(query);

        var quizzesTiny = quizzes.Map(x => x.ToTiny());

        return quizzesTiny;
    }
    
    /// <inheritdoc/>
    public async Task<Quiz?> GetByIdAsync(Guid id)
    {
        return await _quizRepository.GetByIdAsync(id);
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

    private static QuizFile BuildAttachment(IFormFile attachment)
    {
        return new QuizFile
        {
            Status = QuizFileStatus.Uploaded,
            ContentType = attachment.ContentType,
            FileName = attachment.FileName,
            UploadedAt = DateTime.UtcNow,
        };
    }
}
