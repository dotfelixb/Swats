﻿using FluentResults;
using MediatR;
using Swats.Data.Repository;
using Swats.Model;
using Swats.Model.Commands;
using Swats.Model.Queries;

namespace Swats.Infrastructure.Features.Agents.GetAgent;

public class GetAgentCommandHandler : IRequestHandler<GetAgentCommand, Result<SingleResult<FetchAgent>>>
{
    private readonly IAgentRepository _agentRepository;

    public GetAgentCommandHandler(IAgentRepository agentRepository)
    {
        _agentRepository = agentRepository;
    }

    public async Task<Result<SingleResult<FetchAgent>>> Handle(GetAgentCommand request, CancellationToken cancellationToken)
    {
        var result = await _agentRepository.GetAgent(request.Id, cancellationToken);

        return result is null
            ? Result.Fail<SingleResult<FetchAgent>>($"Agent with id [{request.Id}] does not exist!")
            : Result.Ok(new SingleResult<FetchAgent>
            {
                Data = result,
                Ok = true,
                Type = "single"
            });
    }
}