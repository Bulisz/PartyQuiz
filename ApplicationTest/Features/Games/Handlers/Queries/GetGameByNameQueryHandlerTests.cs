using Application.Contracts.Persistence;
using Application.DTOs;
using Application.Exceptions;
using Application.Features.Games.Handlers.Queries;
using Application.Features.Games.Requests.Queries;
using Domain.Games;
using FluentAssertions;
using NSubstitute;

namespace ApplicationTest.Features.Games.Handlers.Queries;

public class GetGameByNameQueryHandlerTests
{
    private readonly IGameRepository _gameRepository = Substitute.For<IGameRepository>();
    private readonly GetGameByNameQueryHandler _handler;

    public GetGameByNameQueryHandlerTests()
    {
        _handler = new GetGameByNameQueryHandler(_gameRepository);
    }

    [Fact]
    public async Task GetGameByNameQueryHandler_ShouldFailure_GameDoesNotExist()
    {
        //Arrange
        _gameRepository.GetGameByNameAsync(Arg.Any<string>())!.Returns(Task.FromResult<Game>(null!));
        var query = new GetGameByNameQuery("Test");

        //Act
        var result = () => _handler.Handle(query, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("gameName");
        exception.Which.Errors[0].Message.Should().Be("Game name does not exist");
    }

    [Fact]
    public async Task GetGameByNameQueryHandler_ShouldSuccesst()
    {
        //Arrange
        _gameRepository.GetGameByNameAsync(Arg.Any<string>()).Returns(c => Game.Create(c[0] as string).Value);
        var query = new GetGameByNameQuery("Test");

        //Act
        var result = await _handler.Handle(query, default);

        //Assert
        result.GetType().Should().Be(typeof(GameResponseDTO));
        result.GameName.Should().Be("Test");
    }
}