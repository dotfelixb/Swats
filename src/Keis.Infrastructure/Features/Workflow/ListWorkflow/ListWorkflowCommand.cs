using FluentResults;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Workflow.ListWorkflow;

public class ListWorkflowCommand : ListType, IRequest<Result<IEnumerable<FetchWorkflow>>>
{
}