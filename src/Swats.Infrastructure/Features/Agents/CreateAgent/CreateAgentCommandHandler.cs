﻿using AutoMapper;
using FluentResults;
using MediatR;
using Swats.Data.Repository;
using Swats.Model.Commands;
using Swats.Model.Domain;
using System.Text.Json;

namespace Swats.Infrastructure.Features.Agents.CreateAgent;

public class CreateAgentCommandHandler : IRequestHandler<CreateAgentCommand, Result<Guid>>
{
    private readonly IAgentRepository _agentRepository;
    private readonly IMapper _mapper;

    public CreateAgentCommandHandler(IAgentRepository agentRepository, IMapper mapper)
    {
        _agentRepository = agentRepository;
        _mapper = mapper;
    }


    public async Task<Result<Guid>> Handle(CreateAgentCommand request, CancellationToken cancellationToken)
    {
        var agent = _mapper.Map<CreateAgentCommand, Agent>(request);

        var auditLog = new DbAuditLog
        {
            Target = agent.Id,
            ActionName = "agent.create",
            Description = "added agent",
            ObjectName = "agent",
            ObjectData = JsonSerializer.Serialize(agent),
            CreatedBy = request.CreatedBy,
        };

        var rst = await _agentRepository.CreateAgent(agent, auditLog, cancellationToken);
        return rst > 0 ? Result.Ok(agent.Id) : Result.Fail<Guid>("Not able to create now!");

    }
}