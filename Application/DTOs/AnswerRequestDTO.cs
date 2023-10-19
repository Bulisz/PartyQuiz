namespace Application.DTOs;

public record AnswerRequestDTO(string AnswerText, bool IsCorrect, string RoundType, string QuestionId);
