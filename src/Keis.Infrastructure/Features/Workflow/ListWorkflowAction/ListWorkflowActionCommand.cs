using FluentResults;
using Keis.Model.Domain;
using MediatR;

namespace Keis.Infrastructure.Features.Workflow.ListWorkflowAction;

public class ListWorkflowActionCommand : IRequest<Result<IEnumerable<WorkflowAction>>>
{
}

