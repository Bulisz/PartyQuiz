using Application.DTOs;
using Domain.Games;

namespace Application.MappingProfiles;

public static class AnswerMappingProfiles
{
    public static Answer ToAnswer(this AnswerRequestDTO answerRequestDTO) =>
        Answer.Create(
            answerRequestDTO.AnswerText,
            answerRequestDTO.IsCorrect,
            Guid.Parse(answerRequestDTO.QuestionId)).Value;

    public static AnswerResponseDTO ToAnswerResponseDTO(this Answer answer) =>
        new (
            answer.Id.ToString(),
            answer.AnswerText,
            answer.IsCorrect);
}
