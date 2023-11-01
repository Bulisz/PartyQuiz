using Application.Contracts.Persistence;
using Application.Contracts.Persistence.Base;
using Application.DTOs;
using Application.Exceptions;
using Application.Features.Games.Handlers.Commands;
using Application.Features.Games.Requests.Commands;
using CSharpFunctionalExtensions;
using Domain.Games;
using FluentAssertions;
using NSubstitute;

namespace ApplicationTest.Features.Games.Handlers.Commands;

public class CreateGameCommandHandlerTests
{
    private readonly IGameRepository _gameRepository = Substitute.For<IGameRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    [Fact]
    public async Task CreateGameCommandHandler_ShouldFailure_WhenGameNameIsEmpty()
    {
        //Arrange
        var dto = new GameRequestDTO("");
        var command = new CreateGameCommand(dto);
        var handler = new CreateGameCommandHandler(_unitOfWork,_gameRepository);

        //Act
        var result = () => handler.Handle(command, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("gameName");
    }

    [Fact]
    public async Task CreateGameCommandHandler_ShouldFailure_WhenGameNameAlreadyExists()
    {
        //Arrange
        var dto = new GameRequestDTO("Exists");
        var command = new CreateGameCommand(dto);
        var handler = new CreateGameCommandHandler(_unitOfWork, _gameRepository);
        var game = Game.Create("Exists").Value;
        _gameRepository.GetGameByNameAsync(Arg.Any<string>()).Returns(Task.FromResult<Game?>(game));

        //Act
        var result = () => handler.Handle(command, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("gameName");
        exception.Which.Errors[0].Message.Should().Be("Game name already exist");
    }

    [Fact]
    public async Task CreateGameCommandHandler_ShouldSuccess()
    {
        //Arrange
        var dto = new GameRequestDTO("Exists");
        var command = new CreateGameCommand(dto);
        var handler = new CreateGameCommandHandler(_unitOfWork, _gameRepository);
        _gameRepository.GetGameByNameAsync(Arg.Any<string>()).Returns(Task.FromResult<Game?>(null));

        //Act
        var result = await handler.Handle(command, default);

        //Assert
        result.Should().BeOfType<GameResponseDTO>();
        result.GameName.Should().Be("Exists");
    }

    [Fact]
    public async Task CreateGameCommandHandler_ShouldCall_RepositoryMethodsProperly()
    {
        //Arrange
        var dto = new GameRequestDTO("Exists");
        var command = new CreateGameCommand(dto);
        var handler = new CreateGameCommandHandler(_unitOfWork, _gameRepository);
        _gameRepository.GetGameByNameAsync(Arg.Any<string>()).Returns(Task.FromResult<Game?>(null));
        await _gameRepository.Add(Arg.Any<Game>());

        //Act
        await handler.Handle(command, default);

        //Assert
        await _gameRepository.Received(1).GetGameByNameAsync("Exists");
        await _gameRepository.Received(1).Add(Arg.Any<Game>());
        await _unitOfWork.Received(1).Save();
    }

    [Fact]
    public async Task CreateGameCommandHandler_ShouldNotCall_RepositoryMethods()
    {
        //Arrange
        var dto = new GameRequestDTO("Exists");
        var command = new CreateGameCommand(dto);
        var handler = new CreateGameCommandHandler(_unitOfWork, _gameRepository);
        var game = Game.Create("Exists").Value;
        _gameRepository.GetGameByNameAsync(Arg.Any<string>()).Returns(Task.FromResult<Game?>(game));
        await _gameRepository.Add(Arg.Any<Game>());

        //Act
        var result = async () => await handler.Handle(command, default);

        //Assert
        await _gameRepository.DidNotReceiveWithAnyArgs().Add(Arg.Any<Game>());
        await _unitOfWork.DidNotReceiveWithAnyArgs().Save();
    }
}