using Application.DTOs;
using Domain.Entities;
using Domain.Enums;

namespace Application.MappingProfiles;

public static class RoundMappingProfile
{
    public static Round ToRound(this RoundRequestDTO roundRequestDTO) =>
        Round.Create(
            roundRequestDTO.RoundNumber,
            roundRequestDTO.RoundName,
            Enum.Parse<RoundType>(roundRequestDTO.RoundType),
            Guid.Parse(roundRequestDTO.GameId));

    public static RoundResponseDTO ToRoundResponseDTO(this Round round)
    {
        var questionList = round.Questions.Select(q => q.ToQuestionResponseDTO()).ToList();

        return new RoundResponseDTO(
            round.Id.ToString(),
            round.RoundNumber,
            round.RoundName,
            round.RoundType.ToString(),
            round.GameId.ToString(),
            questionList);
    }
}
