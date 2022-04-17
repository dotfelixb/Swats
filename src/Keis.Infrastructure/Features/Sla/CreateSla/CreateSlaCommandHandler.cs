using System.Text.Json;
using AutoMapper;
using FluentResults;
using MediatR;
using Keis.Data.Repository;
using Keis.Model.Commands;
using Keis.Model.Domain;

namespace Keis.Infrastructure.Features.Sla.CreateSla;

public class CreateSlaCommand : IRequest<Result<string>>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string BusinessHour { get; set; }
    public int ResponsePeriod { get; set; }
    public DefaultTimeFormat ResponseFormat { get; set; }
    public bool ResponseNotify { get; set; }
    public bool ResponseEmail { get; set; }
    public int ResolvePeriod { get; set; }
    public DefaultTimeFormat ResolveFormat { get; set; }
    public bool ResolveNotify { get; set; }
    public bool ResolveEmail { get; set; }
    public DefaultStatus Status { get; set; }
    public string CreatedBy { get; set; }
}

public class CreateSlaCommandHandler : IRequestHandler<CreateSlaCommand, Result<string>>
{
    private readonly IManageRepository _manageRepository;
    private readonly IMapper _mapper;

    public CreateSlaCommandHandler(IManageRepository manageRepository, IMapper mapper)
    {
        _manageRepository = manageRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateSlaCommand request, CancellationToken cancellationToken)
    {
        var sla = _mapper.Map<CreateSlaCommand, Model.Domain.Sla>(request);

        var auditLog = new DbAuditLog
        {
            Target = sla.Id,
            ActionName = "sla.create",
            Description = "added sla",
            ObjectName = "sla",
            ObjectData = JsonSerializer.Serialize(sla),
            CreatedBy = request.CreatedBy
        };

        var rst = await _manageRepository.CreateSla(sla, auditLog, cancellationToken);
        return rst > 0 ? Result.Ok(sla.Id) : Result.Fail<string>("Not able to create now!");
    }
}