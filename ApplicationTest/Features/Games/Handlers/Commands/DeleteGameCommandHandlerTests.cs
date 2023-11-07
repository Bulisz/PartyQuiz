using Application.Contracts.Persistence;
using Application.Contracts.Persistence.Base;
using Application.Exceptions;
using Application.Features.Games.Handlers.Commands;
using Application.Features.Games.Requests.Commands;
using Domain.Games;
using FluentAssertions;
using NSubstitute;

namespace ApplicationTest.Features.Games.Handlers.Commands;

public class DeleteGameCommandHandlerTests
{
    private readonly IGameRepository _gameRepository = Substitute.For<IGameRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly DeleteGameCommandHandler _handler;

    public DeleteGameCommandHandlerTests()
    {
        _handler = new DeleteGameCommandHandler(_unitOfWork, _gameRepository);
    }

    [Fact]
    public async Task DeleteGameCommandHandler_ShouldFailure_WhenGuidIdNotValid()
    {
        //Arrange
        var command = new DeleteGameCommand("a");

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("gameId");
        exception.Which.Errors[0].Message.Should().Be("a nem valós Id");
    }

    [Fact]
    public async Task DeleteGameCommandHandler_ShouldFailure_WhenGameDoesNotExist()
    {
        //Arrange
        var command = new DeleteGameCommand(Guid.NewGuid().ToString());
        _gameRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Game?>(null));

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("gameId");
        exception.Which.Errors[0].Message.Should().Be("Game Id does not exist");
    }

    [Fact]
    public async Task DeleteGameCommandHandler_ShouldCall_RepositoryMethodsProperly()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var command = new DeleteGameCommand(guid.ToString());
        var game = Game.Create("Exists").Value;
        _gameRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Game?>(game));

        //Act
        await _handler.Handle(command, default);

        //Assert
        await _gameRepository.Received(1).Get(guid);
        _gameRepository.Received(1).Delete(Arg.Any<Game>());
        await _unitOfWork.Received(1).Save();
    }

    [Fact]
    public async Task DeleteGameCommandHandler_ShouldNotCall_RepositoryMethods()
    {
        //Arrange
        var command = new DeleteGameCommand("a");

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        _gameRepository.DidNotReceiveWithAnyArgs().Delete(Arg.Any<Game>());
        await _unitOfWork.DidNotReceiveWithAnyArgs().Save();
    }
}