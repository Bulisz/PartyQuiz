using Application.Contracts.Persistence.Base;
using Application.Contracts.Persistence;
using Application.Features.Games.Handlers.Commands;
using NSubstitute;
using Application.DTOs;
using Application.Exceptions;
using Application.Features.Games.Requests.Commands;
using FluentAssertions;
using Domain.Games;
using NSubstitute.Core.Arguments;

namespace ApplicationTest.Features.Games.Handlers.Commands;

public class UpdateGameCommandHandlerTests
{
    private readonly IGameRepository _gameRepository = Substitute.For<IGameRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly UpdateGameCommandHandler _handler;

    public UpdateGameCommandHandlerTests()
    {
        _handler = new UpdateGameCommandHandler(_unitOfWork, _gameRepository);
    }

    [Fact]
    public async Task CreateGameCommandHandler_ShouldFailure_WhenGameNameIsEmpty()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var dto = new GameUpdateDTO(guid.ToString(),"");
        var command = new UpdateGameCommand(dto);

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("gameName");
    }

    [Fact]
    public async Task CreateGameCommandHandler_ShouldFailure_WhenGameIdIsNotValid()
    {
        //Arrange
        var dto = new GameUpdateDTO("a", "GameName");
        var command = new UpdateGameCommand(dto);

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("gameId");
        exception.Which.Errors[0].Message.Should().Be("a nem valós Id");
    }

    [Fact]
    public async Task CreateGameCommandHandler_ShouldFailure_WhenGameNameAlreadyExist()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var dto = new GameUpdateDTO(guid.ToString(), "GameName");
        var command = new UpdateGameCommand(dto);
        var game = Game.Create("Exists").Value;
        _gameRepository.GetGameByNameAsync(Arg.Any<string>()).Returns(Task.FromResult<Game?>(game));
        _gameRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Game?>(null));

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("gameName");
        exception.Which.Errors[0].Message.Should().Be("Game name already exist");
    }

    [Fact]
    public async Task CreateGameCommandHandler_ShouldFailure_WhenGameDoesNotExist()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var dto = new GameUpdateDTO(guid.ToString(), "GameName");
        var command = new UpdateGameCommand(dto);
        _gameRepository.GetGameByNameAsync(Arg.Any<string>()).Returns(Task.FromResult<Game?>(null));
        _gameRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Game?>(null));

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("gameId");
        exception.Which.Errors[0].Message.Should().Be("This game id does not exist");
    }

    [Fact]
    public async Task CreateGameCommandHandler_ShouldCall_RepositoryMethodsProperly()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var dto = new GameUpdateDTO(guid.ToString(), "GameName");
        var command = new UpdateGameCommand(dto);
        var game = Game.Create("Exists").Value;
        _gameRepository.GetGameByNameAsync(Arg.Any<string>()).Returns(Task.FromResult<Game?>(null));
        _gameRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Game?>(game));

        //Act
        await _handler.Handle(command, default);

        //Assert
        _gameRepository.Received(1).Update(Arg.Any<Game>());
        await _unitOfWork.Received(1).Save();
    }

    [Fact]
    public async Task CreateGameCommandHandler_ShouldNotCall_RepositoryMethodsProperly()
    {
        //Arrange
        var dto = new GameUpdateDTO("a", "");
        var command = new UpdateGameCommand(dto);

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        _gameRepository.DidNotReceiveWithAnyArgs().Update(Arg.Any<Game>());
        await _unitOfWork.DidNotReceiveWithAnyArgs().Save();
    }
}