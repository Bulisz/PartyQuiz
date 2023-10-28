using Application.Contracts.Persistence.Base;
using Domain.Games;

namespace Application.Contracts.Persistence;

public interface IAnswerRepository : IGenericRepository<Answer>
{
    Task<List<Answer>> GetAnswersOfQuestionAsync(string questionId);
}