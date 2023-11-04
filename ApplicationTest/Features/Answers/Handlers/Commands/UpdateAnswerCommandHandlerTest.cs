using Application.Contracts.Persistence;
using Application.Contracts.Persistence.Base;
using Application.DTOs;
using Application.Exceptions;
using Application.Features.Answers.Handlers.Commands;
using Application.Features.Answers.Requests.Commands;
using Domain.Games;
using FluentAssertions;
using NSubstitute;

namespace ApplicationTest.Features.Answers.Handlers.Commands;

public class UpdateAnswerCommandHandlerTest
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IAnswerRepository _answerRepository = Substitute.For<IAnswerRepository>();
    private readonly UpdateAnswerCommandHandler _handler;

    public UpdateAnswerCommandHandlerTest()
    {
        _handler = new UpdateAnswerCommandHandler(_unitOfWork, _answerRepository);
    }

    [Fact]
    public async Task UpdateAnswerCommandHandlerTests_ShouldFailure_AllInputAreInvalid()
    {
        //Arrange
        var dto = new AnswerUpdateDTO("22", "");
        var command = new UpdateAnswerCommand(dto);

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(2);
        exception.Which.Errors[0].Field.Should().Be("answerId");
        exception.Which.Errors[0].Message.Should().Be("22 nem valós Id");
        exception.Which.Errors[1].Field.Should().Be("answerText");
    }

    [Fact]
    public async Task UpdateAnswerCommandHandlerTests_ShouldFailure_AnswerDoesNotExist()
    {
        //Arrange
        var dto = new AnswerUpdateDTO(Guid.Empty.ToString(), "Text");
        var command = new UpdateAnswerCommand(dto);
        _answerRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Answer?>(null));

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("answerId");
        exception.Which.Errors[0].Message.Should().Be("Answer id does not exist");
    }

    [Fact]
    public async Task UpdateAnswerCommandHandlerTests_ShouldCall_RepositoryMethodsProperly()
    {
        //Arrange
        var dto = new AnswerUpdateDTO(Guid.Empty.ToString(), "Text");
        var command = new UpdateAnswerCommand(dto);
        var answer = Answer.Create("OldText", true, Guid.Empty).Value;
        _answerRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Answer?>(answer));

        //Act
        await _handler.Handle(command, default);

        //Assert
        await _answerRepository.Received(1).Get(Arg.Any<Guid>());
        _answerRepository.Received(1).Update(Arg.Any<Answer>());
        await _unitOfWork.Received(1).Save();
    }

    [Fact]
    public async Task UpdateAnswerCommandHandlerTests_ShouldNotCall_RepositoryMethodsProperly()
    {
        //Arrange
        var dto = new AnswerUpdateDTO("22", "");
        var command = new UpdateAnswerCommand(dto);

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        await _answerRepository.DidNotReceiveWithAnyArgs().Get(Arg.Any<Guid>());
        _answerRepository.DidNotReceiveWithAnyArgs().Update(Arg.Any<Answer>());
        await _unitOfWork.DidNotReceiveWithAnyArgs().Save();
    }
}