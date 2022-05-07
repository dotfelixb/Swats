using FluentResults;
using Keis.Data.Repository;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Workflow.GetWorkflow;

public class GetWorkflowCommandHandler : IRequestHandler<GetWorkflowCommand, Result<FetchWorkflow>>
{
    private readonly IManageRepository _manageRepository;

    public GetWorkflowCommandHandler(IManageRepository manageRepository)
    {
        _manageRepository = manageRepository;
    }

    public async Task<Result<FetchWorkflow>> Handle(GetWorkflowCommand request, CancellationToken cancellationToken)
    {
        var rst = await _manageRepository.GetWorkflow(request.Id, cancellationToken);

        return rst is null
            ? Result.Fail<FetchWorkflow>($"Workflow with id [{request.Id}] does not exist!")
            : Result.Ok(rst);
    }
}