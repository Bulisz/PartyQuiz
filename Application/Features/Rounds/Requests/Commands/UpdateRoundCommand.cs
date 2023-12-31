﻿using Application.DTOs;
using MediatR;

namespace Application.Features.Rounds.Requests.Commands;

public class UpdateRoundCommand : IRequest
{
    public RoundUpdateDTO RoundUpdateDTO { get; }

    public UpdateRoundCommand(RoundUpdateDTO roundUpdateDTO)
    {
        RoundUpdateDTO = roundUpdateDTO;
    }
}
