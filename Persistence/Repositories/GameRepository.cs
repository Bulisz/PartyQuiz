using Application.Contracts.Persistence;
using Domain.Entities;
using Persistence.Repositories.Base;

namespace Persistence.Repositories;

public class GameRepository : GenericRepository<Game>, IGameRepository
{
    private readonly PartyQuizDbContext _context;

    public GameRepository(PartyQuizDbContext context) : base(context)
    {
        _context = context;
    }
}
