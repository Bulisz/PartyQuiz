using Persistence.Repositories;
using Persistence;
using Domain.Games;
using FluentAssertions;
using Domain.Enums;

namespace PersistenceTest.Repositories;

public class RoundRepositoryTests
{
    private readonly PartyQuizDbContext _context;
    private readonly RoundRepository _repository;
    public RoundRepositoryTests()
    {
        _context = ContextGenerator.Generate();
        _repository = new RoundRepository(_context);
    }

    [Fact]
    public async Task GetRoundsOfGameAsyncTest()
    {
        var game = Game.Create("NewGame").Value;
        _context.Games.Add(game);
        await _context.SaveChangesAsync();
        var round = Round.Create(1, "RoundName", "ABCD", game.Id).Value;
        var round2 = Round.Create(2, "RN2", "Nullable", game.Id).Value;
        game.TryToAddRound(round);
        game.TryToAddRound(round2);
        _context.Games.Update(game);
        await _context.SaveChangesAsync();
        var question = Question.Create(1, 1, "QestionText", round.Id).Value;
        var question2 = Question.Create(2,2,"QT2", round.Id).Value;
        _context.Questions.Add(question);
        _context.Questions.Add(question2);
        await _context.SaveChangesAsync();


        //Act
        var result = await _repository.GetRoundsOfGameAsync(game.Id.ToString());

        //Assert
        result.Should().HaveCount(2);
        result[0].RoundName.Should().Be("RoundName");
        result[1].RoundType.Should().Be(RoundType.Nullable);
        result[0].Questions.Should().HaveCount(2);
    }
}