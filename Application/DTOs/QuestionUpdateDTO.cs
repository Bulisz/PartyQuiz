using Application.DTOs.Base;

namespace Application.DTOs;

public record QuestionUpdateDTO(
    string Id,
    int QuestionNumber,
    int FullScore,
    string QuestionText) : BaseDTO(Id);
