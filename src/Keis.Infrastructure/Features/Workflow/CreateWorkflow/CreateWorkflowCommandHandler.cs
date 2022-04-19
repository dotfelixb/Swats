using FluentResults;
using MediatR;

namespace Keis.Infrastructure.Features.Workflow.CreateWorkflow;

public class CreateWorkflowCommandHandler : IRequestHandler<CreateWorkflowCommand, Result<string>>
{
    public Task<Result<string>> Handle(CreateWorkflowCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}