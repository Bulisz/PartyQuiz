using Application.Contracts.Persistence;
using Application.Contracts.Persistence.Base;
using Application.DTOs;
using Application.Exceptions;
using Application.Features.Answers.Handlers.Commands;
using Application.Features.Answers.Requests.Commands;
using Domain.Enums;
using Domain.Games;
using FluentAssertions;
using NSubstitute;

namespace ApplicationTest.Features.Answers.Handlers.Commands;

public class CreateAnswerCommandHandlerTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IQuestionRepository _questionRepository = Substitute.For<IQuestionRepository>();
    private readonly IRoundRepository _roundRepository = Substitute.For<IRoundRepository>();
    private readonly CreateAnswerCommandHandler _handler;

    public CreateAnswerCommandHandlerTests()
    {
        _handler = new CreateAnswerCommandHandler(_unitOfWork, _questionRepository, _roundRepository);
    }

    [Fact]
    public async Task CreateAnswerCommandHandlerTests_ShouldFailure_AllInputAreInvalid()
    {
        //Arrange
        var dto = new AnswerRequestDTO("", true, "x", "y");
        var command = new CreateAnswerCommand(dto);

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(3);
        exception.Which.Errors[0].Field.Should().Be("answerText");
        exception.Which.Errors[1].Field.Should().Be("roundId");
        exception.Which.Errors[1].Message.Should().Be("x nem valós Id");
        exception.Which.Errors[2].Field.Should().Be("questionId");
        exception.Which.Errors[2].Message.Should().Be("y nem valós Id");
    }

    [Fact]
    public async Task CreateAnswerCommandHandlerTests_ShouldFailure_QuestionDoesNotExist()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var dto = new AnswerRequestDTO("Text", true, guid.ToString(), guid.ToString());
        var command = new CreateAnswerCommand(dto);
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
    public async Task CreateAnswerCommandHandlerTests_ShouldFailure_RoundDoesNotExist()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var dto = new AnswerRequestDTO("Text", true, guid.ToString(), guid.ToString());
        var command = new CreateAnswerCommand(dto);
        var question = Question.Create(1, 2, "QT", guid).Value;
        _questionRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Question?>(question));
        _roundRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Round?>(null));

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("roundId");
        exception.Which.Errors[0].Message.Should().Be("Round id does not exist");
    }

    [Fact]
    public async Task CreateAnswerCommandHandlerTests_ShouldFailure_QuastionHasTooManyAnswer()
    {
        //Arrange
        var roundGuid = Guid.NewGuid();
        var questionGuid = Guid.NewGuid();
        var dto = new AnswerRequestDTO("Text", false, roundGuid.ToString(), questionGuid.ToString());
        var command = new CreateAnswerCommand(dto);
        var question = Question.Create(1, 2, "QT", roundGuid).Value;
        var answer1 = Answer.Create("Text1", true, Guid.NewGuid()).Value;
        question.TryToAddAnswer(answer1, RoundType.Nullable);
        _questionRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Question?>(question));
        var round = Round.Create(1,"RoundName","Nullable",Guid.NewGuid()).Value;
        _roundRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Round?>(round));

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("answer");
        exception.Which.Errors[0].Message.Should().Be("This type of round has only 1 answer");
    }

    [Fact]
    public async Task CreateAnswerCommandHandlerTests_ShouldFailure_FirstQuestionIsNotCorrect()
    {
        //Arrange
        var roundGuid = Guid.NewGuid();
        var questionGuid = Guid.NewGuid();
        var dto = new AnswerRequestDTO("Text", false, roundGuid.ToString(), questionGuid.ToString());
        var command = new CreateAnswerCommand(dto);
        var question = Question.Create(1, 2, "QT", roundGuid).Value;
        _questionRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Question?>(question));
        var round = Round.Create(1, "RoundName", "Nullable", Guid.NewGuid()).Value;
        _roundRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Round?>(round));

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("answer");
        exception.Which.Errors[0].Message.Should().Be("The first answer must be correct");
    }

    [Fact]
    public async Task CreateAnswerCommandHandlerTests_ShouldFailure_SecondQuestionIsCorrect()
    {
        //Arrange
        var roundGuid = Guid.NewGuid();
        var questionGuid = Guid.NewGuid();
        var dto = new AnswerRequestDTO("Text", true, roundGuid.ToString(), questionGuid.ToString());
        var command = new CreateAnswerCommand(dto);
        var question = Question.Create(1, 2, "QT", roundGuid).Value;
        var answer1 = Answer.Create("Text1", true, Guid.NewGuid()).Value;
        question.TryToAddAnswer(answer1, RoundType.ABCD);
        _questionRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Question?>(question));
        var round = Round.Create(1, "RoundName", "ABCD", Guid.NewGuid()).Value;
        _roundRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Round?>(round));

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("answer");
        exception.Which.Errors[0].Message.Should().Be("Only the first answer can be correct");
    }

    [Fact]
    public async Task CreateAnswerCommandHandlerTests_ShouldFailure_MoreThanFiveAnswer()
    {
        //Arrange
        var roundGuid = Guid.NewGuid();
        var questionGuid = Guid.NewGuid();
        var dto = new AnswerRequestDTO("Text", false, roundGuid.ToString(), questionGuid.ToString());
        var command = new CreateAnswerCommand(dto);
        var question = Question.Create(1, 2, "QT", roundGuid).Value;
        var answer1 = Answer.Create("Text1", true, Guid.NewGuid()).Value;
        var answer2 = Answer.Create("Text2", false, Guid.NewGuid()).Value;
        var answer3 = Answer.Create("Text3", false, Guid.NewGuid()).Value;
        var answer4 = Answer.Create("Text4", false, Guid.NewGuid()).Value;
        var answer5 = Answer.Create("Text5", false, Guid.NewGuid()).Value;
        question.TryToAddAnswer(answer1, RoundType.ABCD);
        question.TryToAddAnswer(answer2, RoundType.ABCD);
        question.TryToAddAnswer(answer3, RoundType.ABCD);
        question.TryToAddAnswer(answer4, RoundType.ABCD);
        question.TryToAddAnswer(answer5, RoundType.ABCD);
        _questionRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Question?>(question));
        var round = Round.Create(1, "RoundName", "ABCD", Guid.NewGuid()).Value;
        _roundRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Round?>(round));

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        var exception = await result.Should().ThrowAsync<QuizValidationException>().WithMessage("Some validation error occurs");
        exception.Which.Errors.Length.Should().Be(1);
        exception.Which.Errors[0].Field.Should().Be("answer");
        exception.Which.Errors[0].Message.Should().Be("Maximum answer ammount is 5");
    }

    [Fact]
    public async Task CreateAnswerCommandHandlerTests_ShouldSuccess()
    {
        //Arrange
        var roundGuid = Guid.NewGuid();
        var questionGuid = Guid.NewGuid();
        var dto = new AnswerRequestDTO("Text", true, roundGuid.ToString(), questionGuid.ToString());
        var command = new CreateAnswerCommand(dto);
        var question = Question.Create(1, 2, "QT", roundGuid).Value;
        _questionRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Question?>(question));
        var round = Round.Create(1, "RoundName", "Nullable", Guid.NewGuid()).Value;
        _roundRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Round?>(round));

        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        result.GetType().Should().Be(typeof(AnswerResponseDTO));
        result.AnswerText.Should().Be("Text");
        result.IsCorrect.Should().Be(true);
    }

    [Fact]
    public async Task CreateAnswerCommandHandlerTests_ShouldCall_RepositoryMethodsProperly()
    {
        //Arrange
        var roundGuid = Guid.NewGuid();
        var questionGuid = Guid.NewGuid();
        var dto = new AnswerRequestDTO("Text", true, roundGuid.ToString(), questionGuid.ToString());
        var command = new CreateAnswerCommand(dto);
        var question = Question.Create(1, 2, "QT", roundGuid).Value;
        _questionRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Question?>(question));
        var round = Round.Create(1, "RoundName", "Nullable", Guid.NewGuid()).Value;
        _roundRepository.Get(Arg.Any<Guid>()).Returns(Task.FromResult<Round?>(round));

        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        await _questionRepository.Received(1).Get(questionGuid);
        await _roundRepository.Received(1).Get(roundGuid);
        _questionRepository.Received(1).Update(question);
        await _unitOfWork.Received(1).Save();
    }

    [Fact]
    public async Task CreateAnswerCommandHandlerTests_ShouldNotCall_RepositoryMethodsProperly()
    {
        //Arrange
        var dto = new AnswerRequestDTO("", true, "x", "y");
        var command = new CreateAnswerCommand(dto);

        //Act
        var result = () => _handler.Handle(command, default);

        //Assert
        await _questionRepository.DidNotReceiveWithAnyArgs().Get(Arg.Any<Guid>());
        await _roundRepository.DidNotReceiveWithAnyArgs().Get(Arg.Any<Guid>());
        _questionRepository.DidNotReceiveWithAnyArgs().Update(Arg.Any<Question>());
        await _unitOfWork.DidNotReceiveWithAnyArgs().Save();
    }
}