using Application.Contracts.Persistence;
using Domain.Games;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Base;

namespace Persistence.Repositories;

public class GameRepository : GenericRepository<Game>, IGameRepository
{
    private readonly PartyQuizDbContext _context;

    public GameRepository(PartyQuizDbContext context) : base(context)
    {
        _context = context;
    }

    public new async Task<Game?> Get(Guid id)
    {
        return await _context.Games
            .Include(g => g.Rounds)
            .ThenInclude(r => r.Questions)
            .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Game>> GetAllGameNames()
    {
        return await _context.Games.ToListAsync();
    }

    public async Task<Game?> GetGameByNameAsync(string gameName)
    {
        return await _context.Games
            .Include(g => g.Rounds)
            .ThenInclude(r => r.Questions)
            .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(g => g.GameName == gameName);
    }
}
