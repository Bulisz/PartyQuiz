using Application.DTOs.Base;

namespace Application.DTOs;

public record AnswerResponseDTO(string Id, string AnswerText, bool IsCorrect) : BaseDTO(Id);
