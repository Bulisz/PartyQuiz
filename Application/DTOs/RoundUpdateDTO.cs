﻿using Application.DTOs.Base;

namespace Application.DTOs;

public record RoundUpdateDTO(
    string Id,
    int RoundNumber,
    string RoundName,
    string RoundType,
    string GameId) : BaseDTO(Id);
