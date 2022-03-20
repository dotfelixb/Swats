using AutoMapper;
using FluentResults;
using MediatR;
using Swats.Data.Repository;
using Swats.Infrastructure.Extensions;
using Swats.Model;
using Swats.Model.Commands;
using Swats.Model.Domain;
using System.Text.Json;

namespace Swats.Infrastructure.Features.Department.CreateDepartment;

public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, Result<string>>
{
    private readonly IManageRepository _manageRepository;
    private readonly IMapper _mapper;

    public CreateDepartmentCommandHandler(IManageRepository manageRepository, IMapper mapper)
    {
        _manageRepository = manageRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var code = await _manageRepository.GenerateDepartmentCode(cancellationToken);

        var department = _mapper.Map<CreateDepartmentCommand, Model.Domain.Department>(request);
        department.Code = code.FormatCode(prefix: "DT");

        var auditLog = new DbAuditLog
        {
            Target = department.Id,
            ActionName = "department.create",
            Description = "added department hour",
            ObjectName = "department",
            ObjectData = JsonSerializer.Serialize(department),
            CreatedBy = request.CreatedBy,
        };

        var rst = await _manageRepository.CreateDepartment(department, auditLog, cancellationToken);
        return rst > 0 ? Result.Ok(department.Id) : Result.Fail<string>("Not able to create now!");
    }
}
