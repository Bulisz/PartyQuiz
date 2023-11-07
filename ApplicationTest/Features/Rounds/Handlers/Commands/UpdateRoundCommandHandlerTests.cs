using Application.Contracts.Persistence.Base;
using Application.Contracts.Persistence;
using Application.Features.Rounds.Handlers.Commands;
using NSubstitute;
using Application.DTOs;
using Application.Exceptions;
using Application.Features.Rounds.Requests.Commands;
using FluentAssertions;
using Domain.Games;

namespace ApplicationTest.Features.Rounds.Handlers.Commands;

public class UpdateRoundCommandHandlerTests
{
    private readonly IGameRepository _gameRepository = Substitute.For<IGameRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly UpdateRoundCommandHandler _handler;

    public UpdateRoundCommandHandlerTests()
    {
        _handler = new UpdateRoundCommandHandler(_unitOfWork, _gameRepository);
    }

    [Fact]
    public async Task UpdateRoundCommandHandlerTests_ShouldFailure_AllInputAreInvalid()
    {
        //Arrange
        var dto = new RoundUpdateDTO("22", 11, "", "ABC", "34");
        var command = new UpdateRoundCommand(dto);

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(5);
        exception.Which.Errors[0].Field.Should().Be("roundId");
        exception.Which.Errors[0].Message.Should().Be("22 nem valós Id");
        exception.Which.Errors[1].Field.Should().Be("gameId");
        exception.Which.Errors[1].Message.Should().Be("34 nem valós Id");
        exception.Which.Errors[2].Field.Should().Be("roundNumber");
        exception.Which.Errors[3].Field.Should().Be("roundName");
        exception.Which.Errors[4].Field.Should().Be("roundType");
        exception.Which.Errors[4].Message.Should().Be("ABC nem valós kör típus");
    }

    [Fact]
    public async Task UpdateRoundCommandHandlerTests_ShouldFailure_GameDoesNotExist()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var dto = new RoundUpdateDTO(guid.ToString(), 9, "RoundName", "ABCD", guid.ToString());
        var command = new UpdateRoundCommand(dto);
        _gameRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Game?>(null));

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("gameId");
        exception.Which.Errors[0].Message.Should().Be("Game id does not exist");
    }

    [Fact]
    public async Task UpdateRoundCommandHandlerTests_ShouldFailure_RoundDoesNotExist()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var dto = new RoundUpdateDTO(guid.ToString(), 9, "RoundName", "ABCD", guid.ToString());
        var command = new UpdateRoundCommand(dto);
        var game = Game.Create("GameName").Value;
        _gameRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Game?>(game));

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("roundId");
        exception.Which.Errors[0].Message.Should().Be("Round id does not exist");
    }

    [Fact]
    public async Task UpdateRoundCommandHandlerTests_ShouldFailure_RoundNumberAlreadyExist()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var dto = new RoundUpdateDTO(Guid.Empty.ToString(), 2, "RoundName", "ABCD", guid.ToString());
        var command = new UpdateRoundCommand(dto);
        var game = Game.Create("GameName").Value;
        var roundToModify = Round.Create(1, "RoundName", "ABCD", guid).Value;
        var anotherRound = Round.Create(2, "RoundName2", "Nullable", guid).Value;
        anotherRound.Modify(guid);
        game.TryToAddRound(anotherRound);
        game.TryToAddRound(roundToModify);
        _gameRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Game?>(game));

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("round");
        exception.Which.Errors[0].Message.Should().Be("Round number already exist in this game");
    }

    [Fact]
    public async Task UpdateRoundCommandHandlerTests_ShouldFailure_RoundNameAlreadyExist()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var dto = new RoundUpdateDTO(Guid.Empty.ToString(), 1, "RoundName2", "ABCD", guid.ToString());
        var command = new UpdateRoundCommand(dto);
        var game = Game.Create("GameName").Value;
        var roundToModify = Round.Create(1, "RoundName", "ABCD", guid).Value;
        var anotherRound = Round.Create(2, "RoundName2", "Nullable", guid).Value;
        anotherRound.Modify(guid);
        game.TryToAddRound(anotherRound);
        game.TryToAddRound(roundToModify);
        _gameRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Game?>(game));

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("round");
        exception.Which.Errors[0].Message.Should().Be("Round name already exist in this game");
    }

    [Fact]
    public async Task UpdateRoundCommandHandlerTests_ShouldCall_RepositoryMethodsProperly()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var dto = new RoundUpdateDTO(Guid.Empty.ToString(), 9, "RoundName", "ABCD", guid.ToString());
        var command = new UpdateRoundCommand(dto);
        var game = Game.Create("GameName").Value;
        game.TryToAddRound(Round.Create(2, "RoundName2", "Nullable", guid).Value);
        _gameRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Game?>(game));

        //Act
        await _handler.Handle(command, default);

        //Assert
        _gameRepository.Received(1).Update(Arg.Any<Game>());
        await _unitOfWork.Received(1).Save();
    }

    [Fact]
    public async Task UpdateRoundCommandHandlerTests_ShouldNotCall_RepositoryMethodsProperly()
    {
        //Arrange
        var dto = new RoundUpdateDTO("22", 11, "", "ABC", "34");
        var command = new UpdateRoundCommand(dto);

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        _gameRepository.DidNotReceiveWithAnyArgs().Update(Arg.Any<Game>());
        await _unitOfWork.DidNotReceiveWithAnyArgs().Save();
    }
}