using Domain.Entities.Base;

namespace Application.Contracts.Persistence.Base;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> Get(Guid id);
    Task<IReadOnlyList<T>> GetAll();
    Task<T> Add(T entity);
    Task<bool> Exists(Guid id);
    void Update(T entity);
    void Delete(T entity);
}
