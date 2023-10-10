using Application.Contracts.Persistence.Base;
using Domain.Entities;

namespace Application.Contracts.Persistence;

public interface IRoundRepository : IGenericRepository<Round>
{
    Task<List<Round>> GetRoundsOfGame(string gameId);
}
