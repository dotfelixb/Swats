using FluentResults;
using Keis.Model.Domain;
using MediatR;

namespace Keis.Infrastructure.Features.Workflow.ListWorkflowAction;

public class
    ListWorkflowActionCommandHandler : IRequestHandler<ListWorkflowActionCommand, Result<IEnumerable<WorkflowAction>>>
{
    public Task<Result<IEnumerable<WorkflowAction>>> Handle(ListWorkflowActionCommand request,
        CancellationToken cancellationToken)
    {
        // we intentionally return this list
        IEnumerable<WorkflowAction> actions = new List<WorkflowAction>
        {
            new() {Name = "Forward To", ActionFrom = ActionType.ForwardTo, Control = ControlType.Input, Link = null},
            new()
            {
                Name = "Assign To", ActionFrom = ActionType.AssignTo, Control = ControlType.Select,
                Link = "methods/agent.list"
            },
            new()
            {
                Name = "Assign Department", ActionFrom = ActionType.AssignDepartment, Control = ControlType.Select,
                Link = "methods/department.list"
            },
            new()
            {
                Name = "Assign Team", ActionFrom = ActionType.AssignTeam, Control = ControlType.Select,
                Link = "methods/team.list"
            },
            new()
            {
                Name = "Apply SLA", ActionFrom = ActionType.ApplySla, Control = ControlType.Select,
                Link = "methods/sla.list"
            },
            new()
            {
                Name = "Change Status", ActionFrom = ActionType.ChangeStatus, Control = ControlType.Select, Link = null
            }
        };

        return Task.FromResult(Result.Ok(actions));
    }
}