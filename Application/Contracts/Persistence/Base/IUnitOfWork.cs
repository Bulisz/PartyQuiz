namespace Application.Contracts.Persistence.Base;

public interface IUnitOfWork : IDisposable
{
    IGameRepository GameRepository { get; }
    IRoundRepository RoundRepository { get; }
    IQuestionRepository QuestionRepository { get; }
    IAnswerRepository AnswerRepository { get; }
    Task Save();
}
