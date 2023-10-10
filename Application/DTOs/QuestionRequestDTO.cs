namespace Application.DTOs;

public record QuestionRequestDTO(int QuestionNumber, int FullScore, string QuestionText, string RoundId);
