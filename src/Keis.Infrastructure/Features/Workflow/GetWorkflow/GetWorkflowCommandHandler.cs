using FluentResults;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Workflow.GetWorkflow;

public class GetWorkflowCommandHandler : IRequestHandler<GetWorkflowCommand, Result<FetchWorkflow>>
{
    public Task<Result<FetchWorkflow>> Handle(GetWorkflowCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}