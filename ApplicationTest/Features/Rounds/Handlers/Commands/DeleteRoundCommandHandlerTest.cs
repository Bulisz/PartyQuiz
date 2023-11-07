using Application.Contracts.Persistence;
using Application.Contracts.Persistence.Base;
using Application.Exceptions;
using Application.Features.Rounds.Handlers.Commands;
using Application.Features.Rounds.Requests.Commands;
using Domain.Games;
using FluentAssertions;
using NSubstitute;

namespace ApplicationTest.Features.Rounds.Handlers.Commands;

public class DeleteRoundCommandHandlerTest
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IRoundRepository _roundRepository = Substitute.For<IRoundRepository>();
    private readonly DeleteRoundCommandHandler _handler;

    public DeleteRoundCommandHandlerTest()
    {
        _handler = new DeleteRoundCommandHandler(_unitOfWork, _roundRepository);
    }

    [Fact]
    public async Task DeleteRoundCommandHandler_ShouldFailure_IdIsNotValid()
    {
        //Arrange
        var command = new DeleteRoundCommand("a");

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("roundId");
        exception.Which.Errors[0].Message.Should().Be("a nem valós Id");
    }

    [Fact]
    public async Task DeleteRoundCommandHandler_ShouldFailure_RoundDoesNotExist()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var command = new DeleteRoundCommand(guid.ToString());
        _roundRepository.Get(Arg.Any<Guid>())!.Returns(Task.FromResult<Round>(null!));

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("roundId");
        exception.Which.Errors[0].Message.Should().Be("Round Id does not exist");
    }

    [Fact]
    public async Task DeleteRoundCommandHandler_ShouldCall_RepositoryMethodsProperly()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var command = new DeleteRoundCommand(guid.ToString());
        var round = Round.Create(1, "RoundName", "ABCD", guid).Value;
        _roundRepository.Get(Arg.Any<Guid>())!.Returns(Task.FromResult(round));

        //Act
        await _handler.Handle(command, default);

        //Assert
        await _roundRepository.Received(1).Get(guid);
        _roundRepository.Received(1).Delete(Arg.Any<Round>());
        await _unitOfWork.Received(1).Save();
    }

    [Fact]
    public async Task DeleteRoundCommandHandler_ShouldNotCall_RepositoryMethodsProperly()
    {
        //Arrange
        var command = new DeleteRoundCommand("a");

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        await _roundRepository.DidNotReceiveWithAnyArgs().Get(Arg.Any<Guid>());
        _roundRepository.DidNotReceiveWithAnyArgs().Delete(Arg.Any<Round>());
        await _unitOfWork.DidNotReceiveWithAnyArgs().Save();
    }
}