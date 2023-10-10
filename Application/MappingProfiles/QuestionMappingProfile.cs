﻿using Application.DTOs;
using Domain.Entities;

namespace Application.MappingProfiles;

public static class QuestionMappingProfile
{
    public static Question ToQuestion(this QuestionRequestDTO questionRequestDTO) =>
        Question.Create(
            questionRequestDTO.QuestionNumber,
            questionRequestDTO.FullScore,
            questionRequestDTO.QuestionText,
            Guid.Parse(questionRequestDTO.RoundId));

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
