using Persistence.Repositories;
using Persistence;
using Domain.Games;
using FluentAssertions;

namespace PersistenceTest.Repositories;

public class AnswerRepositoryTests
{
    private readonly PartyQuizDbContext _context;
    private readonly AnswerRepository _repository;
    public AnswerRepositoryTests()
    {
        _context = ContextGenerator.Generate();
        _repository = new AnswerRepository(_context);
    }

    [Fact]
    public async Task GetAnswersOfQuestionAsync_Test()
    {
        //Arrange
        var game = Game.Create("NewGame").Value;
        _context.Games.Add(game);
        await _context.SaveChangesAsync();
        var round = Round.Create(1, "RoundName", "ABCD", game.Id).Value;
        game.TryToAddRound(round);
        _context.Games.Update(game);
        await _context.SaveChangesAsync();
        var question = Question.Create(1, 1, "QestionText", round.Id).Value;
        _context.Questions.Add(question);
        await _context.SaveChangesAsync();
        var answer = Answer.Create("AnswerText", true, question.Id).Value;
        var answer2 = Answer.Create("AT2", false, question.Id).Value;
        _context.Answers.Add(answer);
        _context.Answers.Add(answer2);
        await _context.SaveChangesAsync();

        //Act
        var result = await _repository.GetAnswersOfQuestionAsync(question.Id.ToString());

        //Assert
        result.Should().HaveCount(2);
        result[0].IsCorrect.Should().BeTrue();
        result[1].AnswerText.Should().Be("AT2");
    }
}