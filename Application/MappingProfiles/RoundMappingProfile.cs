using Application.DTOs;
using Domain.Games;

namespace Application.MappingProfiles;

public static class RoundMappingProfile
{
    public static Round ToRound(this RoundRequestDTO roundRequestDTO) =>
        Round.Create(
            roundRequestDTO.RoundNumber,
            roundRequestDTO.RoundName,
            roundRequestDTO.RoundType,
            Guid.Parse(roundRequestDTO.GameId)).Value;

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

    public static Round ToRound(this RoundUpdateDTO roundUpdateDTO) =>
        Round.Create(
            roundUpdateDTO.RoundNumber,
            roundUpdateDTO.RoundName,
            roundUpdateDTO.RoundType,
            Guid.Parse(roundUpdateDTO.GameId)).Value;
}
