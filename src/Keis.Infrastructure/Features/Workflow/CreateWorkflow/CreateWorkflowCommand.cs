using FluentResults;
using Keis.Model.Domain;
using MediatR;

namespace Keis.Infrastructure.Features.Workflow.CreateWorkflow;

public class CreateWorkflowCommand : IRequest<Result<string>>
{
    public string Name { get; set; }
    public WorkflowEvent[] Events { get; set; }
    public WorkflowCriteria[] Criterias { get; set; }
    public WorkflowAction[] Actions { get; set; }
    public string Description { get; set; }
    public string CreatedBy { get; set; }
    public DefaultStatus Status { get; set; } = DefaultStatus.Active;
}