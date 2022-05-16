using System.Text.Json;
using AutoMapper;
using FluentResults;
using Keis.Data.Repository;
using Keis.Model.Domain;
using MediatR;

namespace Keis.Infrastructure.Features.Workflow.CreateWorkflow;

public class CreateWorkflowCommandHandler : IRequestHandler<CreateWorkflowCommand, Result<string>>
{
    private readonly IManageRepository _manageRepository;
    private readonly IMapper _mapper;

    public CreateWorkflowCommandHandler(IManageRepository manageRepository, IMapper mapper)
    {
        _manageRepository = manageRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateWorkflowCommand request, CancellationToken cancellationToken)
    {
        var workflow = _mapper.Map<CreateWorkflowCommand, Model.Domain.Workflow>(request);

        var auditLog = new DbAuditLog
        {
            Target = workflow.Id,
            ActionName = "workflow.create",
            Description = "added workflow",
            ObjectName = "workflow",
            ObjectData = JsonSerializer.Serialize(workflow),
            CreatedBy = request.CreatedBy
        };

        // TODO: v2 move to transaction
        var rst = await _manageRepository.CreateWorkflow(workflow, auditLog, cancellationToken);
        return rst > 2 ? Result.Ok(workflow.Id) : Result.Fail<string>("Not able to create now!");
    }
}