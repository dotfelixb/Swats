using FluentResults;
using Keis.Model.Domain;
using MediatR;

namespace Keis.Infrastructure.Features.Workflow.ListWorkflowEvent;

public class ListWorkflowEventCommandHandler : IRequestHandler<ListWorkflowEventCommand, Result<IEnumerable<WorkflowEvent>>>
{
    public Task<Result<IEnumerable<WorkflowEvent>>> Handle(ListWorkflowEventCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}