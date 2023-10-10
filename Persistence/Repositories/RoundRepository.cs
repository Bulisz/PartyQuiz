using Application.Contracts.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Base;

namespace Persistence.Repositories;

public class RoundRepository : GenericRepository<Round>, IRoundRepository
{
    private readonly PartyQuizDbContext _context;

    public RoundRepository(PartyQuizDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Round>> GetRoundsOfGameAsync(string gameId)
    {
        var a = await _context.Rounds
            .Include(r => r.Questions)
            .ThenInclude(q => q.Answers)
            .Where(r => r.GameId == Guid.Parse(gameId))
            .ToListAsync();

        return a;
    }
}
