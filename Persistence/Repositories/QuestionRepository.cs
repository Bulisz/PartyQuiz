using Application.Contracts.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Base;

namespace Persistence.Repositories;

public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
{
    private readonly PartyQuizDbContext _context;

    public QuestionRepository(PartyQuizDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Question>> GetQuestionsOfRound(string roundId)
    {
        return await _context.Questions.Where(q => q.RoundId.ToString() == roundId).ToListAsync();
    }
}
