using Application.Contracts.Persistence.Base;
using Domain.Entities;

namespace Application.Contracts.Persistence;

public interface IQuestionRepository : IGenericRepository<Question>
{
    Task<List<Question>> GetQuestionsOfRoundAsync(string roundId);
}
