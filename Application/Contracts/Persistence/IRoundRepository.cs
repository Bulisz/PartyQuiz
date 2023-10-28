using Application.Contracts.Persistence.Base;
using Domain.Games;

namespace Application.Contracts.Persistence;

public interface IRoundRepository : IGenericRepository<Round>
{
    Task<List<Round>> GetRoundsOfGameAsync(string gameId);
}