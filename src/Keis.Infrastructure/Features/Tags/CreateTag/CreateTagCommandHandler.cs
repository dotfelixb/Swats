﻿using AutoMapper;
using FluentResults;
using MediatR;
using Keis.Data.Repository;
using Keis.Model.Commands;
using Keis.Model.Domain;
using System.Text.Json;

namespace Keis.Infrastructure.Features.Tags.CreateTag;

public class CreateTagCommand : IRequest<Result<string>>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
    public DefaultType Visibility { get; set; }
    public DefaultStatus Status { get; set; }
    public string CreatedBy { get; set; }
}

public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, Result<string>>
{
    private readonly IManageRepository _manageRepository;
    private readonly IMapper _mapper;

    public CreateTagCommandHandler(IManageRepository manageRepository, IMapper mapper)
    {
        _manageRepository = manageRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        var tag = _mapper.Map<CreateTagCommand, Tag>(request);

        var auditLog = new DbAuditLog
        {
            Target = tag.Id,
            ActionName = "tag.create",
            Description = "added tag hour",
            ObjectName = "tag",
            ObjectData = JsonSerializer.Serialize(tag),
            CreatedBy = request.CreatedBy
        };

        var rst = await _manageRepository.CreateTag(tag, auditLog, cancellationToken);
        return rst > 0 ? Result.Ok(tag.Id) : Result.Fail<string>("Not able to create now!");
    }
}