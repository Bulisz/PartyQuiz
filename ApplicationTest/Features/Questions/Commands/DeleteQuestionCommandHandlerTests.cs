using Application.Contracts.Persistence;
using Application.Contracts.Persistence.Base;
using Application.Exceptions;
using Application.Features.Questions.Handlers.Commands;
using Application.Features.Questions.Requests.Commands;
using Domain.Games;
using FluentAssertions;
using NSubstitute;

namespace ApplicationTest.Features.Questions.Commands;

public class DeleteQuestionCommandHandlerTests
{
    private readonly IQuestionRepository _questionRepository = Substitute.For<IQuestionRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly DeleteQuestionCommandHandler _handler;

    public DeleteQuestionCommandHandlerTests()
    {
        _handler = new DeleteQuestionCommandHandler(_unitOfWork, _questionRepository);
    }

    [Fact]
    public async Task DeleteQuestionCommandHandler_ShouldFailure_IdIsNotValid()
    {
        //Arrange
        var command = new DeleteQuestionCommand("a");

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("questionId");
        exception.Which.Errors[0].Message.Should().Be("a nem valós Id");
    }

    [Fact]
    public async Task DeleteQuestionCommandHandler_ShouldFailure_RoundDoesNotExist()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var command = new DeleteQuestionCommand(guid.ToString());
        _questionRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Question?>(null));

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("questionId");
        exception.Which.Errors[0].Message.Should().Be("Question id does not exist");
    }

    [Fact]
    public async Task DeleteQuestionCommandHandler_ShouldCall_RepositoryMethodsProperly()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var command = new DeleteQuestionCommand(guid.ToString());
        var question = Question.Create(1,3,"Text",guid).Value;
        _questionRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Question?>(question));

        //Act
        await _handler.Handle(command, default);

        //Assert
        await _questionRepository.Received(1).Get(guid);
        _questionRepository.Received(1).Delete(Arg.Any<Question>());
        await _unitOfWork.Received(1).Save();
    }

    [Fact]
    public async Task DeleteQuestionCommandHandler_ShouldNotCall_RepositoryMethodsProperly()
    {
        //Arrange
        var command = new DeleteQuestionCommand("a");

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        await _questionRepository.DidNotReceiveWithAnyArgs().Get(Arg.Any<Guid>());
        _questionRepository.DidNotReceiveWithAnyArgs().Delete(Arg.Any<Question>());
        await _unitOfWork.DidNotReceiveWithAnyArgs().Save();
    }
}