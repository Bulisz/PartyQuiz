using Application.Contracts.Persistence.Base;
using Domain.Entities;

namespace Application.Contracts.Persistence;

public interface IAnswerRepository : IGenericRepository<Answer>
{
    Task<List<Answer>> GetAnswersOfQuestion(string questionId);
}
