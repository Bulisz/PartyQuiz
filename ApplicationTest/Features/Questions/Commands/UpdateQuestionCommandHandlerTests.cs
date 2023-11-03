using Application.Contracts.Persistence;
using Application.Contracts.Persistence.Base;
using Application.DTOs;
using Application.Exceptions;
using Application.Features.Questions.Handlers.Commands;
using Application.Features.Questions.Requests.Commands;
using Domain.Games;
using FluentAssertions;
using NSubstitute;

namespace ApplicationTest.Features.Questions.Commands;

public class UpdateQuestionCommandHandlerTests
{
    private readonly IQuestionRepository _questionRepository = Substitute.For<IQuestionRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly UpdateQuestionCommandHandler _handler;

    public UpdateQuestionCommandHandlerTests()
    {
        _handler = new UpdateQuestionCommandHandler(_unitOfWork, _questionRepository);
    }

    [Fact]
    public async Task UpdateQuestionCommandHandlerTests_ShouldFailure_AllInputAreInvalid()
    {
        //Arrange
        var dto = new QuestionUpdateDTO("22", 11, "");
        var command = new UpdateQuestionCommand(dto);

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(3);
        exception.Which.Errors[0].Field.Should().Be("questionId");
        exception.Which.Errors[0].Message.Should().Be("22 nem valós Id");
        exception.Which.Errors[1].Field.Should().Be("fullScore");
        exception.Which.Errors[2].Field.Should().Be("questionText");
    }

    [Fact]
    public async Task UpdateQuestionCommandHandlerTests_ShouldFailure_RoundDoesNotExist()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var dto = new QuestionUpdateDTO(guid.ToString(), 5, "Text");
        var command = new UpdateQuestionCommand(dto);
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
    public async Task UpdateQuestionCommandHandlerTests_ShouldCall_RepositoryMethodsProperly()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var dto = new QuestionUpdateDTO(guid.ToString(), 5, "Text");
        var command = new UpdateQuestionCommand(dto);
        var question = Question.Create(1, 4, "Test", Guid.Empty).Value;
        _questionRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Question?>(question));

        //Act
        await _handler.Handle(command, default);

        //Assert
        _questionRepository.Received(1).Update(Arg.Any<Question>());
        await _unitOfWork.Received(1).Save();
    }

    [Fact]
    public async Task UpdateQuestionCommandHandlerTests_ShouldNotCall_RepositoryMethodsProperly()
    {
        //Arrange
        var dto = new QuestionUpdateDTO("22", 11, "");
        var command = new UpdateQuestionCommand(dto);

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        _questionRepository.DidNotReceiveWithAnyArgs().Update(Arg.Any<Question>());
        await _unitOfWork.DidNotReceiveWithAnyArgs().Save();
    }
}