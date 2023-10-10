using Application.Contracts.Persistence.Base;
using Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories.Base;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly PartyQuizDbContext _context;

    public GenericRepository(PartyQuizDbContext context)
    {
        _context = context;
    }

    public async Task<T?> Get(Guid id)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IReadOnlyList<T>> GetAll()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T> Add(T entity)
    {
        await _context.AddAsync(entity);
        return entity;
    }

    public async Task<bool> Exists(Guid id)
    {
        var entity = await Get(id);
        return entity != null;
    }

    public void Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
}
