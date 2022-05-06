using FluentResults;
using Keis.Data.Repository;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Workflow.ListWorkflow;

public class ListWorkflowCommandHandler : IRequestHandler<ListWorkflowCommand, Result<IEnumerable<FetchWorkflow>>>
{
    private readonly IManageRepository _manageRepository;

    public ListWorkflowCommandHandler(IManageRepository manageRepository)
    {
        _manageRepository = manageRepository;
    }

    public async Task<Result<IEnumerable<FetchWorkflow>>> Handle(ListWorkflowCommand request, CancellationToken cancellationToken)
    {
        var rst = await _manageRepository.ListWorkflow(request.Offset, request.Limit, request.Deleted,
            cancellationToken);

        return Result.Ok(rst);
    }
}