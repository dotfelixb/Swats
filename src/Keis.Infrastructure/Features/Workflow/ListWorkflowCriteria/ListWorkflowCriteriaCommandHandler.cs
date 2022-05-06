using FluentResults;
using Keis.Model.Domain;
using MediatR;

namespace Keis.Infrastructure.Features.Workflow.ListWorkflowCriteria;

public class
    ListWorkflowCriteriaCommandHandler : IRequestHandler<ListWorkflowCriteriaCommand,
        Result<IEnumerable<WorkflowCriteria>>>
{
    public Task<Result<IEnumerable<WorkflowCriteria>>> Handle(ListWorkflowCriteriaCommand request,
        CancellationToken cancellationToken)
    {
        // we intentionally return this list
        IEnumerable<WorkflowCriteria> criterias = new List<WorkflowCriteria>
        {
            new() {Name = "Subject", Criteria = CriteriaType.Subject, Control = ControlType.Input, Link = null},
            new()
            {
                Name = "Department", Criteria = CriteriaType.Department, Control = ControlType.Select,
                Link = "methods/department.list"
            },
            new()
            {
                Name = "Team", Criteria = CriteriaType.Team, Control = ControlType.Select, Link = "methods/team.list"
            }
        };

        return Task.FromResult(Result.Ok(criterias));
    }
}