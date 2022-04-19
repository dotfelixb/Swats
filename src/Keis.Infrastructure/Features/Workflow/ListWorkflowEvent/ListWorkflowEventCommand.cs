using FluentResults;
using Keis.Model.Domain;
using MediatR;

namespace Keis.Infrastructure.Features.Workflow.ListWorkflowEvent;

public class ListWorkflowEventCommand : IRequest<Result<IEnumerable<WorkflowEvent>>>
{
}