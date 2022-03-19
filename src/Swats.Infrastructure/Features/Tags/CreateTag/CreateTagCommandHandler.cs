﻿using AutoMapper;
using FluentResults;
using MediatR;
using Swats.Data.Repository;
using Swats.Model.Commands;
using Swats.Model.Domain;
using System.Text.Json;

namespace Swats.Infrastructure.Features.Tags.CreateTag;

public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, Result<Guid>>
{
    private readonly IManageRepository _manageRepository;
    private readonly IMapper _mapper;

    public CreateTagCommandHandler(IManageRepository manageRepository, IMapper mapper)
    {
        _manageRepository = manageRepository;
        _mapper = mapper;

    }
    public async Task<Result<Guid>> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        var tag = _mapper.Map<CreateTagCommand, Tag>(request);

        var auditLog = new DbAuditLog
        {
            Target = tag.Id,
            ActionName = "businesshour.create",
            Description = "added business hour",
            ObjectName = "tickettype",
            ObjectData = JsonSerializer.Serialize(tag),
            CreatedBy = request.CreatedBy,
        };

        var rst = await _manageRepository.CreateTag(tag, auditLog, cancellationToken);
        return rst > 0 ? Result.Ok(tag.Id) : Result.Fail<Guid>("Not able to create now!");
    }
}