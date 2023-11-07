using Application.Contracts.Persistence.Base;

namespace Persistence.Repositories.Base;

public class UnitOfWork : IUnitOfWork
{
    private readonly PartyQuizDbContext _context;

    public UnitOfWork(PartyQuizDbContext context)
    {
        _context = context;
    }

    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

}
