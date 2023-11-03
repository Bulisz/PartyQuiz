using Application.Contracts.Persistence;
using Application.DTOs;
using Application.Exceptions;
using Application.Features.Rounds.Handlers.Queries;
using Application.Features.Rounds.Requests.Queries;
using Domain.Games;
using FluentAssertions;
using NSubstitute;

namespace ApplicationTest.Features.Rounds.Handlers.Queries;

public class GetRoundsOfGameQueryHandlerTests
{
    private readonly IGameRepository _gameRepository = Substitute.For<IGameRepository>();
    private readonly IRoundRepository _roundRepository = Substitute.For<IRoundRepository>();
    private readonly GetRoundsOfGameQueryHandler _handler;

    public GetRoundsOfGameQueryHandlerTests()
    {
        _handler = new GetRoundsOfGameQueryHandler(_roundRepository, _gameRepository);
    }

    [Fact]
    public async Task GetRoundsOfGameQueryHandler_ShouldFailure_GameIdIsNotValid()
    {
        //Arrange
        var query = new GetRoundsOfGameQuery("34");

        //Act
        var result = () => _handler.Handle(query, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("gameId");
        exception.Which.Errors[0].Message.Should().Be("34 nem valós Id");
    }

    [Fact]
    public async Task GetRoundsOfGameQueryHandler_ShouldFailure_GameDoesNotExist()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var query = new GetRoundsOfGameQuery(guid.ToString());
        _gameRepository.Get(Arg.Any<Guid>())!.Returns(Task.FromResult<Game>(null!));

        //Act
        var result = () => _handler.Handle(query, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("gameId");
        exception.Which.Errors[0].Message.Should().Be("Game does not exist");
    }

    [Fact]
    public async Task GetRoundsOfGameQueryHandler_ShouldSuccess()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var query = new GetRoundsOfGameQuery(guid.ToString());
        var game = Game.Create(guid.ToString()).Value;
        var roundToModify = Round.Create(1, "RoundName", "ABCD", guid).Value;
        var anotherRound = Round.Create(2, "RoundName2", "Nullable", guid).Value;
        game.TryToAddRound(roundToModify);
        game.TryToAddRound(anotherRound);
        _gameRepository.Get(Arg.Any<Guid>())!.Returns(Task.FromResult(game));
        var rounds = game.Rounds.ToList();
        _roundRepository.GetRoundsOfGameAsync(Arg.Any<string>()).Returns(Task.FromResult(rounds));

        //Act
        var result = await _handler.Handle(query, default);

        //Assert
        result.GetType().Should().Be(typeof(List<RoundResponseDTO>));
        result[0].RoundNumber.Should().Be(1);
        result[1].RoundName.Should().Be("RoundName2");
    }
}