using MediatR;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Questions.Commands.UpdateQuestion
{
    /// <summary>
    /// Represents a command handler to update a question.
    /// </summary>
    public class UpdateQuestionCommandHandler(IQuestionService questionService) : IRequestHandler<UpdateQuestionCommand, QuizWorldResponse<Question>>
    {
        private readonly IQuestionService _questionService = questionService;
        
        /// <inheritdoc/>
        public async Task<QuizWorldResponse<Question>> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = await _questionService.UpdateQuestion(request);
            return QuizWorldResponse<Question>.Success(question);
        }
    }
}
