using System.Text.Json;
using AutoMapper;
using FluentResults;
using Keis.Data.Repository;
using Keis.Model.Domain;
using MediatR;

namespace Keis.Infrastructure.Features.Agents.CreateAgent;

public class CreateAgentCommandHandler : IRequestHandler<CreateAgentCommand, Result<string>>
{
    private readonly IAgentRepository _agentRepository;
    private readonly IMapper _mapper;

    public CreateAgentCommandHandler(IAgentRepository agentRepository, IMapper mapper)
    {
        _agentRepository = agentRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateAgentCommand request, CancellationToken cancellationToken)
    {
        var agent = _mapper.Map<CreateAgentCommand, Agent>(request);

        var rst = await _agentRepository.CreateAgent(agent, cancellationToken);
        return rst > 0 ? Result.Ok(agent.Id) : Result.Fail<string>("Not able to create now!");
    }
}