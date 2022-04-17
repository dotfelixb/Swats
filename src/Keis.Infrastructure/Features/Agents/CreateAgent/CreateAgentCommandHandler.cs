using AutoMapper;
using FluentResults;
using MediatR;
using Keis.Data.Repository;
using Keis.Model.Commands;
using Keis.Model.Domain;
using System.Text.Json;
using MassTransit;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Keis.Infrastructure.Features.Agents.CreateAgent;

public class CreateAgentCommand : IRequest<Result<string>>
{
    public string Id { get; set; } = NewId.NextGuid().ToString(); // need for agent and user creation
    public string UserName { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Mobile { get; set; }
    public string Telephone { get; set; }
    public string Timezone { get; set; }
    public string Department { get; set; }
    public IEnumerable<SelectListItem> DepartmentList { get; set; } = Enumerable.Empty<SelectListItem>();
    public string Team { get; set; }
    public IEnumerable<SelectListItem> TeamList { get; set; } = Enumerable.Empty<SelectListItem>();
    public string Type { get; set; }
    public IEnumerable<SelectListItem> TypeList { get; set; } = Enumerable.Empty<SelectListItem>();
    public AgentMode Mode { get; set; }
    public string CreatedBy { get; set; }
    public string Note { get; set; }
}

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

        var auditLog = new DbAuditLog
        {
            Target = agent.Id,
            ActionName = "agent.create",
            Description = "added agent",
            ObjectName = "agent",
            ObjectData = JsonSerializer.Serialize(agent),
            CreatedBy = request.CreatedBy
        };

        var rst = await _agentRepository.CreateAgent(agent, auditLog, cancellationToken);
        return rst > 0 ? Result.Ok(agent.Id) : Result.Fail<string>("Not able to create now!");
    }
}