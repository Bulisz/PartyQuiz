using Application.DTOs;
using Domain.Games;

namespace Application.MappingProfiles;

public static class QuestionMappingProfile
{
    public static Question ToQuestion(this QuestionRequestDTO questionRequestDTO, int questionNumber) =>
        Question.Create(
            questionNumber,
            questionRequestDTO.FullScore,
            questionRequestDTO.QuestionText,
            Guid.Parse(questionRequestDTO.RoundId)).Value;

    public static QuestionResponseDTO ToQuestionResponseDTO(this Question question)
    {
        var answerList = question.Answers.Select(a => a.ToAnswerResponseDTO()).ToList();

        return new QuestionResponseDTO(
            question.Id.ToString(),
            question.QuestionNumber,
            question.FullScore,
            question.QuestionText,
            question.RoundId.ToString(),
            answerList);
    }
}
