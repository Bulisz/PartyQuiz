using Application.Contracts.Persistence;
using Domain.Games;
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

    public async Task<List<Question>> GetQuestionsOfRoundAsync(string roundId)
    {
        return await _context.Questions
            .Include(q => q.Answers)
            .Where(q => q.RoundId == Guid.Parse(roundId))
            .ToListAsync();
    }

    public new async Task<Question?> Get(Guid id)
    {
        return await _context.Questions
            .Include(q => q.Answers)
            .FirstOrDefaultAsync(q => q.Id == id);
    }
}
