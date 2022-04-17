using AutoMapper;
using FluentResults;
using MediatR;
using Keis.Data.Repository;
using Keis.Model.Commands;
using Keis.Model.Domain;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Keis.Infrastructure.Features.HelpTopic.CreateHelpTopic;

public class CreateHelpTopicCommand : IRequest<Result<string>>
{
    public string Topic { get; set; }
    public DefaultType Type { get; set; }
    public DefaultStatus Status { get; set; }
    public string Department { get; set; }
    public IEnumerable<SelectListItem> DepartmentList { get; set; } = Enumerable.Empty<SelectListItem>();
    public string DefaultDepartment { get; set; }
    public string Note { get; set; }
    public string CreatedBy { get; set; }
}

public class CreateHelpTopicCommandHandler : IRequestHandler<CreateHelpTopicCommand, Result<string>>
{
    private readonly IManageRepository _manageRepository;
    private readonly IMapper _mapper;

    public CreateHelpTopicCommandHandler(IManageRepository manageRepository, IMapper mapper)
    {
        _manageRepository = manageRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateHelpTopicCommand request, CancellationToken cancellationToken)
    {
        var helpTopic = _mapper.Map<CreateHelpTopicCommand, Model.Domain.HelpTopic>(request);

        var auditLog = new DbAuditLog
        {
            Target = helpTopic.Id,
            ActionName = "helptopic.create",
            Description = "added help topic",
            ObjectName = "helptopic",
            ObjectData = JsonSerializer.Serialize(helpTopic),
            CreatedBy = request.CreatedBy
        };

        var rst = await _manageRepository.CreateHelpTopic(helpTopic, auditLog, cancellationToken);
        return rst > 0 ? Result.Ok(helpTopic.Id) : Result.Fail<string>("Not able to create now!");
    }
}