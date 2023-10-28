namespace Application.Contracts.Persistence.Base;

public interface IUnitOfWork : IDisposable
{
    Task Save();
}
