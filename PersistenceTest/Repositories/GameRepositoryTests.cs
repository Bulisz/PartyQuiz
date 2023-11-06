using Domain.Games;
using FluentAssertions;
using Persistence;
using Persistence.Repositories;

namespace PersistenceTest.Repositories;

public class GameRepositoryTests
{
    private readonly PartyQuizDbContext _context;
    private readonly GameRepository _repository;
    public GameRepositoryTests()
    {
        _context = ContextGenerator.Generate();
        _repository = new GameRepository(_context);
    }

    [Fact]
    public async Task Get_Test()
    {
        //Arrange
        var game = Game.Create("NewGame").Value;
        _context.Games.Add(game);
        await _context.SaveChangesAsync();
        var round = Round.Create(1, "RoundName", "ABCD", game.Id).Value;
        game.TryToAddRound(round);
        _context.Games.Update(game);
        await _context.SaveChangesAsync();
        var question = Question.Create(1, 1, "QestionText",round.Id).Value;
        _context.Questions.Add(question);
        await _context.SaveChangesAsync();
        var answer = Answer.Create("AnswerText", true, question.Id).Value;
        _context.Answers.Add(answer);
        await _context.SaveChangesAsync();
        

        //Act
        var result = await _repository.Get(game.Id);

        //Assert
        result!.GameName.Should().Be("NewGame");
        result.Id.Should().Be(game.Id);
        result.Rounds.Should().HaveCount(1);
        result.Rounds.ToList()[0].Questions.Should().HaveCount(1);
        result.Rounds.ToList()[0].Questions.ToList()[0].Answers.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetAll_Test()
    {
        //Arrange
        var game1 = Game.Create("NewGame1").Value;
        var game2 = Game.Create("NewGame2").Value;
        _context.Games.Add(game1);
        _context.Games.Add(game2);
        await _context.SaveChangesAsync();

        //Act
        var result = await _repository.GetAll();

        //Assert
        result.Should().HaveCount(2);
        result[1].GameName.Should().Be("NewGame2");
    }

    [Fact]
    public async Task Add_Test()
    {
        //Arrange
        var game = Game.Create("NewGame").Value;

        //Act
        var result = await _repository.Add(game);
        await _context.SaveChangesAsync();

        //Assert
        result.GameName.Should().Be("NewGame");
        result.Id.Should().NotBe(Guid.Empty);
        _context.Games.Should().HaveCount(1);
    }

    [Fact]
    public async Task Exists_Test()
    {
        //Arrange
        var game = Game.Create("NewGame").Value;
        _context.Games.Add(game);
        await _context.SaveChangesAsync();

        //Act
        var result = await _repository.Exists(game.Id);

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Update_Test()
    {
        //Arrange
        var game = Game.Create("NewGame").Value;
        _context.Games.Add(game);
        await _context.SaveChangesAsync();
        game.Modify("AnotherGame");

        //Act
        _repository.Update(game);
        await _context.SaveChangesAsync();
        var result = await _context.Games.FindAsync(game.Id);

        //Assert
        result!.GameName.Should().Be("AnotherGame");
    }

    [Fact]
    public async Task Delete_Test()
    {
        //Arrange
        var game = Game.Create("NewGame").Value;
        _context.Games.Add(game);
        await _context.SaveChangesAsync();

        //Act
        _repository.Delete(game);
        await _context.SaveChangesAsync();
        var result = _context.Games.ToList();

        //Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllGameNames_Test()
    {
        //Arrange
        var game1 = Game.Create("NewGame1").Value;
        var game2 = Game.Create("NewGame2").Value;
        _context.Games.Add(game1);
        _context.Games.Add(game2);
        await _context.SaveChangesAsync();

        //Act
        var result = await _repository.GetAllGameNames();

        //Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetGameByName_Test()
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
        _context.Answers.Add(answer);
        await _context.SaveChangesAsync();

        //Act
        var result = await _repository.GetGameByNameAsync("NewGame");

        //Assert
        result!.GameName.Should().Be("NewGame");
        result.Id.Should().Be(game.Id);
        result.Rounds.ToList().Should().HaveCount(1);
        result.Rounds.ToList()[0].Questions.Should().HaveCount(1);
        result.Rounds.ToList()[0].Questions.ToList()[0].Answers.Should().HaveCount(1);
    }
}