using System;
using FluentResults;
using Keis.Model.Domain;
using MediatR;

namespace Keis.Infrastructure.Features.Workflow.ListWorkflowCriteria;

public class ListWorkflowCriteriaCommand : IRequest<Result<IEnumerable<WorkflowCriteria>>>
{
}
