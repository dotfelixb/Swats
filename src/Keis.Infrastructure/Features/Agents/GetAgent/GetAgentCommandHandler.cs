﻿using FluentResults;
using MediatR;
using Keis.Data.Repository;
using Keis.Model;
using Keis.Model.Commands;
using Keis.Model.Queries;

namespace Keis.Infrastructure.Features.Agents.GetAgent;

public class GetAgentCommand : GetType, IRequest<Result<FetchAgent>>
{
}

public class GetAgentCommandHandler : IRequestHandler<GetAgentCommand, Result<FetchAgent>>
{
    private readonly IAgentRepository _agentRepository;

    public GetAgentCommandHandler(IAgentRepository agentRepository)
    {
        _agentRepository = agentRepository;
    }

    public async Task<Result<FetchAgent>> Handle(GetAgentCommand request, CancellationToken cancellationToken)
    {
        var result = await _agentRepository.GetAgent(request.Id, cancellationToken);

        return result is null
            ? Result.Fail<FetchAgent>($"Agent with id [{request.Id}] does not exist!")
            : Result.Ok(result);
    }
}