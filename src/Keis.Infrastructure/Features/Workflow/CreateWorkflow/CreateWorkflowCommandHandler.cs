using FluentResults;
using MediatR;

namespace Keis.Infrastructure.Features.Workflow.CreateWorkflow;

public class CreateWorkflowCommand : IRequest<Result<string>>
{
    public string CreatedBy { get; set; }
}

public class CreateWorkflowCommandHandler : IRequestHandler<CreateWorkflowCommand, Result<string>>
{
    public Task<Result<string>> Handle(CreateWorkflowCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}