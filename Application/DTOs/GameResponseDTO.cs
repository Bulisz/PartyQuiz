using Application.DTOs.Base;

namespace Application.DTOs;

public record GameResponseDTO(string Id, string GameName, List<RoundResponseDTO> Rounds) : BaseDTO(Id);
