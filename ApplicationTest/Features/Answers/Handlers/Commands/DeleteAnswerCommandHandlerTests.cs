using Application.Contracts.Persistence;
using Application.Contracts.Persistence.Base;
using Application.Exceptions;
using Application.Features.Answers.Handlers.Commands;
using Application.Features.Answers.Requests.Commands;
using Domain.Games;
using FluentAssertions;
using NSubstitute;

namespace ApplicationTest.Features.Answers.Handlers.Commands;

public class DeleteAnswerCommandHandlerTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IAnswerRepository _answerRepository = Substitute.For<IAnswerRepository>();
    private readonly DeleteAnswerCommandHandler _handler;

    public DeleteAnswerCommandHandlerTests()
    {
        _handler = new DeleteAnswerCommandHandler(_unitOfWork, _answerRepository);
    }

    [Fact]
    public async Task DeleteAnswerCommandHandler_ShouldFailure_IdIsNotValid()
    {
        //Arrange
        var command = new DeleteAnswerCommand("a");

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("answerId");
        exception.Which.Errors[0].Message.Should().Be("a nem valós Id");
    }

    [Fact]
    public async Task DeleteAnswerCommandHandler_ShouldFailure_AnswerDoesNotExist()
    {
        //Arrange
        var command = new DeleteAnswerCommand(Guid.Empty.ToString());
        _answerRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Answer?>(null));

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("answerId");
        exception.Which.Errors[0].Message.Should().Be("Answer Id does not exist");
    }

    [Fact]
    public async Task DeleteAnswerCommandHandler_ShouldCall_RepositoryMethodsProperly()
    {
        //Arrange
        var command = new DeleteAnswerCommand(Guid.Empty.ToString());
        var answer = Answer.Create("Text", true, Guid.Empty).Value;
        _answerRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Answer?>(answer));
        
        //Act
        await _handler.Handle(command, default);

        //Assert
        await _answerRepository.Received(1).Get(Arg.Any<Guid>());
        _answerRepository.Received(1).Delete(Arg.Any<Answer>());
        await _unitOfWork.Received(1).Save();
    }

    [Fact]
    public async Task DeleteAnswerCommandHandler_ShouldNotCall_RepositoryMethodsProperly()
    {
        //Arrange
        var command = new DeleteAnswerCommand("a");

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        await _answerRepository.DidNotReceiveWithAnyArgs().Get(Arg.Any<Guid>());
        _answerRepository.DidNotReceiveWithAnyArgs().Delete(Arg.Any<Answer>());
        await _unitOfWork.DidNotReceiveWithAnyArgs().Save();
    }
}