using AutoMapper;
using FluentResults;
using MediatR;
using Keis.Data.Repository;
using Keis.Model.Commands;
using Keis.Model.Domain;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Keis.Infrastructure.Features.Department.CreateDepartment;

public class CreateDepartmentCommand : IRequest<Result<string>>
{
    public string Name { get; set; }
    public string Manager { get; set; }
    public IEnumerable<SelectListItem> ManagerList { get; set; } = Enumerable.Empty<SelectListItem>();
    public string BusinessHour { get; set; }
    public IEnumerable<SelectListItem> BusinessHours { get; set; } = Enumerable.Empty<SelectListItem>();
    public string OutgoingEmail { get; set; }
    public DefaultType Type { get; set; }
    public DefaultStatus Status { get; set; }
    public string Response { get; set; }
    public string CreatedBy { get; set; }
}

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
        var department = _mapper.Map<CreateDepartmentCommand, Model.Domain.Department>(request);

        var auditLog = new DbAuditLog
        {
            Target = department.Id,
            ActionName = "department.create",
            Description = "added department",
            ObjectName = "department",
            ObjectData = JsonSerializer.Serialize(department),
            CreatedBy = request.CreatedBy
        };

        var rst = await _manageRepository.CreateDepartment(department, auditLog, cancellationToken);
        return rst > 0 ? Result.Ok(department.Id) : Result.Fail<string>("Not able to create now!");
    }
}