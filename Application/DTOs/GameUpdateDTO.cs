using Application.DTOs.Base;

namespace Application.DTOs;

public record GameUpdateDTO(string Id, string GameName) : BaseDTO(Id);
