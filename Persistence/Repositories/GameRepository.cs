using Application.Contracts.Persistence;
using Domain.Entities;
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

    public async Task<Game?> GetGameByNameAsync(string gameName)
    {
        return await _context.Games
            .Include(g => g.Rounds)
            .ThenInclude(r => r.Questions)
            .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(g => g.GameName == gameName);
    }
}
