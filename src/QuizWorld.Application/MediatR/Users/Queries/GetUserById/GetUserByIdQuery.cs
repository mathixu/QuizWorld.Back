using QuizWorld.Application.MediatR.Common;
using QuizWorld.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWorld.Application.MediatR.Users.Queries.GetUserById;

public record GetUserByIdQuery(Guid UserId) : IQuizWorldRequest<User>;
