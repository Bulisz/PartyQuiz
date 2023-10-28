using Application.Contracts.Persistence.Base;
using Domain.Games;

namespace Application.Contracts.Persistence;

public interface IGameRepository : IGenericRepository<Game>
{
    Task<IEnumerable<Game>> GetAllGameNames();
    Task<Game?> GetGameByNameAsync(string GameName);
}
