using Application.DTOs.Base;

namespace Application.DTOs;

public record RoundResponseDTO(
    string Id,
    int RoundNumber,
    string RoundName,
    string RoundType,
    string GameId,
    List<QuestionResponseDTO> Questions) : BaseDTO(Id);
