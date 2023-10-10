using Application.Contracts.Persistence;
using Application.Contracts.Persistence.Base;

namespace Persistence.Repositories.Base;

public class UnitOfWork : IUnitOfWork
{
    private readonly PartyQuizDbContext _context;
    private IGameRepository? _gameRepository;
    private IRoundRepository? _roundRepository;
    private IQuestionRepository? _questionRepository;
    private IAnswerRepository? _answerRepository;

    public UnitOfWork(PartyQuizDbContext context)
    {
        _context = context;
    }

    public IGameRepository GameRepository => _gameRepository ??= new GameRepository(_context);
    public IRoundRepository RoundRepository => _roundRepository ??= new RoundRepository(_context);
    public IQuestionRepository QuestionRepository => _questionRepository ??= new QuestionRepository(_context);
    public IAnswerRepository AnswerRepository => _answerRepository ??= new AnswerRepository(_context);

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
