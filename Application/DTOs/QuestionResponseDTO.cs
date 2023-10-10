using Application.DTOs.Base;

namespace Application.DTOs;

public record QuestionResponseDTO(
    string Id,
    int QuestionNumber,
    int FullScore,
    string QuestionText,
    string RoundId,
    List<AnswerResponseDTO> Answers) : BaseDTO(Id);
