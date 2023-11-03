using Application.Contracts.Persistence;
using Application.DTOs;
using Application.Features.Games.Handlers.Queries;
using Application.Features.Games.Requests.Queries;
using Domain.Games;
using FluentAssertions;
using NSubstitute;

namespace ApplicationTest.Features.Games.Handlers.Queries;

public class GetAllGameNamesQueryHandlerTests
{
    private readonly IGameRepository _gameRepository = Substitute.For<IGameRepository>();
    private readonly GetAllGameNamesQueryHandler _handler;

    public GetAllGameNamesQueryHandlerTests()
    {
        _handler = new GetAllGameNamesQueryHandler(_gameRepository);
    }

    [Fact]
    public async Task GetAllGameNamesQueryHandler_ShouldSuccess_ReturnWithList()
    {
        //Arrange
        var games = new List<Game>()
        {
            Game.Create("Name1").Value,
            Game.Create("Name2").Value
        };
        _gameRepository.GetAllGameNames().Returns(games);
        var query = new GetAllGameNamesQuery();

        //Act
        var result = await _handler.Handle(query, default);
        
        //Assert
        result.GetType().Should().Be(typeof(List<GameResponseDTO>));
        result.Count.Should().Be(2);
        result[0].GameName.Should().Be("Name1");
    }

    [Fact]
    public async Task GetAllGameNamesQueryHandler_ShouldSuccess_ReturnWithEmptyList()
    {
        //Arrange
        var games = new List<Game>(){};
        _gameRepository.GetAllGameNames().Returns(games);
        var query = new GetAllGameNamesQuery();

        //Act
        var result = await _handler.Handle(query, default);

        //Assert
        result.GetType().Should().Be(typeof(List<GameResponseDTO>));
        result.Should().BeEmpty();
    }
}