using FluentResults;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Workflow.ListWorkflow;

public class ListWorkflowCommand  : ListType, IRequest<Result<IEnumerable<FetchWorkflow>>>{}

public class ListWorkflowCommandHandler: IRequestHandler<ListWorkflowCommand, Result<IEnumerable<FetchWorkflow>>>
{
    public Task<Result<IEnumerable<FetchWorkflow>>> Handle(ListWorkflowCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}