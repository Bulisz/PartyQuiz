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
        return await _context.Rounds.Where(r => r.GameId.ToString() == gameId).ToListAsync();
    }
}
