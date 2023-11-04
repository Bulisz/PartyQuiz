using Domain.Games;
using FluentAssertions;
using Persistence;
using Persistence.Repositories;

namespace PersistenceTest.Repositories;

public class GameRepositoryTest
{
    private readonly PartyQuizDbContext _context;
    private readonly GameRepository _repository;
    public GameRepositoryTest()
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

        //Act
        var result = await _repository.Get(game.Id);

        //Assert
        result!.GameName.Should().Be("NewGame");
        result.Id.Should().Be(game.Id);
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

        //Act
        var result = await _repository.GetGameByNameAsync("NewGame");

        //Assert
        result!.GameName.Should().Be("NewGame");
    }
}