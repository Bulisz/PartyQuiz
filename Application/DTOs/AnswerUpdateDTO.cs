using Application.DTOs.Base;

namespace Application.DTOs;

public record AnswerUpdateDTO(
    string Id,
    string AnswerText) : BaseDTO(Id);
