using CSharpFunctionalExtensions;
using Domain.Enums;

namespace Domain.Games;

public class Question : Entity<Guid>
{
    public int QuestionNumber { get; private init; }
    public int FullScore { get; private set; }
    public string QuestionText { get; private set; } = string.Empty;
    public Guid RoundId { get; private init; }
    public Round Round { get; private init; } = null!;
    public IReadOnlyCollection<Answer> Answers => _answers;
    private readonly List<Answer> _answers  = new();

    private Question(int questionNumber, int fullScore, string questionText, Guid roundId)
    {
        QuestionNumber = questionNumber;
        FullScore = fullScore;
        QuestionText = questionText;
        RoundId = roundId;
    }

    public static Result<Question> Create(int questionNumber, int fullScore, string questionText, Guid roundId)
    {
        var Question = new Question(questionNumber, fullScore, questionText, roundId);

        return Question;
    }

    public Result Modify(int fullScore, string questionText)
    {
        FullScore = fullScore;
        QuestionText = questionText;

        return Result.Success();
    }

    public Result TryToAddAnswer(Answer answer, RoundType roundType)
    {
        if (roundType != RoundType.ABCD && _answers.Count > 0)
            return Result.Failure("This type of round has only 1 answer");

        if(_answers.Count == 0 && !answer.IsCorrect)
            return Result.Failure("The first answer must be correct");

        if (_answers.Count > 0 && answer.IsCorrect)
            return Result.Failure("Only the first answer can be correct");

        if (_answers.Count == 5)
            return Result.Failure("Maximum answer ammount is 5");

        _answers.Add(answer);

        return Result.Success();
    }
}