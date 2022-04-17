using AutoMapper;
using FluentResults;
using MediatR;
using Keis.Data.Repository;
using Keis.Model.Commands;
using Keis.Model.Domain;
using System.Text.Json;

namespace Keis.Infrastructure.Features.BusinessHour.CreateBusinessHour;

public class CreateBusinessHourCommand : IRequest<Result<string>>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Timezone { get; set; }
    public DefaultStatus Status { get; set; }
    public OpenHour[] OpenHours { get; set; } = Array.Empty<OpenHour>();
    public string CreatedBy { get; set; }
}

public class CreateBusinessHourCommandHandler : IRequestHandler<CreateBusinessHourCommand, Result<string>>
{
    private readonly IManageRepository _manageRepository;
    private readonly IMapper _mapper;

    public CreateBusinessHourCommandHandler(IManageRepository manageRepository, IMapper mapper)
    {
        _manageRepository = manageRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateBusinessHourCommand request, CancellationToken cancellationToken)
    {
        var businessHour = _mapper.Map<CreateBusinessHourCommand, Model.Domain.BusinessHour>(request);

        var auditLog = new DbAuditLog
        {
            Target = businessHour.Id,
            ActionName = "businesshour.create",
            Description = "added business hour",
            ObjectName = "tickettype",
            ObjectData = JsonSerializer.Serialize(businessHour),
            CreatedBy = request.CreatedBy
        };

        var rst = await _manageRepository.CreateBusinessHour(businessHour, auditLog, cancellationToken);
        return rst > 0 ? Result.Ok(businessHour.Id) : Result.Fail<string>("Not able to create now!");
    }
}