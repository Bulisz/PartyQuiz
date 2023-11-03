using Application.Contracts.Persistence;
using Application.Contracts.Persistence.Base;
using Application.DTOs;
using Application.Exceptions;
using Application.Features.Rounds.Handlers.Commands;
using Application.Features.Rounds.Requests.Commands;
using Domain.Games;
using FluentAssertions;
using NSubstitute;

namespace ApplicationTest.Features.Rounds.Handlers.Commands;

public class CreateRoundCommandHandlerTests
{
    private readonly IGameRepository _gameRepository = Substitute.For<IGameRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly CreateRoundCommandHandler _handler;

    public CreateRoundCommandHandlerTests()
    {
        _handler = new CreateRoundCommandHandler(_unitOfWork, _gameRepository);
    }

    [Fact]
    public async Task CreateRoundCommandHandlerTests_ShouldFailure_AllInputAreInvalid()
    {
        //Arrange
        var dto = new RoundRequestDTO(11, "", "ABC", "34");
        var command = new CreateRoundCommand(dto);

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(4);
        exception.Which.Errors[0].Field.Should().Be("gameId");
        exception.Which.Errors[0].Message.Should().Be("34 nem valós Id");
        exception.Which.Errors[1].Field.Should().Be("roundType");
        exception.Which.Errors[1].Message.Should().Be("ABC nem valós kör típus");
        exception.Which.Errors[2].Field.Should().Be("roundNumber");
        exception.Which.Errors[3].Field.Should().Be("roundName");
    }

    [Fact]
    public async Task CreateRoundCommandHandlerTests_ShouldFailure_GameDoesNotExist()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var dto = new RoundRequestDTO(9, "RoundName", "ABCD", guid.ToString());
        var command = new CreateRoundCommand(dto);
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
    public async Task CreateRoundCommandHandlerTests_ShouldFailure_RoundNumberAlreadyExist()
    {
        //Arrange
        var gameGuid = Guid.NewGuid();
        var roundGuid = Guid.NewGuid();
        var dto = new RoundRequestDTO(9, "RoundName", "ABCD", gameGuid.ToString());
        var command = new CreateRoundCommand(dto);
        var game = Game.Create("GameName").Value;
        game.TryToAddRound(Round.Create(9,"RoundName2","Nullable", gameGuid).Value);
        _gameRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Game?>(game));

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("round");
        exception.Which.Errors[0].Message.Should().Be("Round number already exists in this game");
    }

    [Fact]
    public async Task CreateRoundCommandHandlerTests_ShouldFailure_RoundNameAlreadyExist()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var dto = new RoundRequestDTO(9, "RoundName", "ABCD", guid.ToString());
        var command = new CreateRoundCommand(dto);
        var game = Game.Create("GameName").Value;
        game.TryToAddRound(Round.Create(1, "RoundName", "Nullable", guid).Value);
        _gameRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Game?>(game));

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("round");
        exception.Which.Errors[0].Message.Should().Be("Round name already exists in this game");
    }

    [Fact]
    public async Task CreateRoundCommandHandlerTests_ShouldSuccess()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var dto = new RoundRequestDTO(9, "RoundName", "ABCD", guid.ToString());
        var command = new CreateRoundCommand(dto);
        var game = Game.Create("GameName").Value;
        game.TryToAddRound(Round.Create(1, "RoundName2", "Nullable", guid).Value);
        _gameRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Game?>(game));

        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        result.GetType().Should().Be(typeof(RoundResponseDTO));
        result.RoundName.Should().Be("RoundName");
        result.RoundType.Should().Be("ABCD");
    }

    [Fact]
    public async Task CreateRoundCommandHandlerTests_ShouldCall_RepositoryMethodsProperly()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var dto = new RoundRequestDTO(9, "RoundName", "ABCD", guid.ToString());
        var command = new CreateRoundCommand(dto);
        var game = Game.Create("GameName").Value;
        game.TryToAddRound(Round.Create(1, "RoundName2", "Nullable", guid).Value);
        _gameRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Game?>(game));

        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        await _gameRepository.Received(1).Get(guid);
        _gameRepository.Received(1).Update(Arg.Any<Game>());
        await _unitOfWork.Received(1).Save();
    }

    [Fact]
    public async Task CreateRoundCommandHandlerTests_ShouldNotCall_RepositoryMethodsProperly()
    {
        //Arrange
        var dto = new RoundRequestDTO(11, "", "ABC", "34");
        var command = new CreateRoundCommand(dto);

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        await _gameRepository.DidNotReceiveWithAnyArgs().Get(Arg.Any<Guid>());
        _gameRepository.DidNotReceiveWithAnyArgs().Update(Arg.Any<Game>());
        await _unitOfWork.DidNotReceiveWithAnyArgs().Save();

    }
}