using AutoMapper;
using FluentResults;
using Keis.Data.Repository;
using Keis.Model.Domain;
using MediatR;
using System.Text.Json;

namespace Keis.Infrastructure.Features.HelpTopic.CreateHelpTopic;

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