using FluentResults;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Workflow.GetWorkflow;

public class GetWorkflowCommand : GetType, IRequest<Result<FetchWorkflow>>
{
}