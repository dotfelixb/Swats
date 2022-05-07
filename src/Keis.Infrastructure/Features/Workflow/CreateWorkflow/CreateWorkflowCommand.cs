using FluentResults;
using Keis.Model.Domain;
using MediatR;

namespace Keis.Infrastructure.Features.Workflow.CreateWorkflow;

public class CreateWorkflowCommand : IRequest<Result<string>>
{
    public string Name { get; set; }
    public EventType[] Events { get; set; }
    public WorkflowCriteria[] Criteria { get; set; }
    public WorkflowAction[] Actions { get; set; }
    public WorkflowPriority Priority { get; set; }
    public string Note { get; set; }
    public string CreatedBy { get; set; }
    public DefaultStatus Status { get; set; } = DefaultStatus.Active;
}