using Application.Contracts.Persistence.Base;
using Domain.Entities;

namespace Application.Contracts.Persistence;

public interface IGameRepository : IGenericRepository<Game>
{
}
