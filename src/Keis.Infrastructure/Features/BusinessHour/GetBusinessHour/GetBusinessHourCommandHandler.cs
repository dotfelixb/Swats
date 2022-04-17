﻿using FluentResults;
using MediatR;
using Keis.Data.Repository;
using Keis.Model.Commands;
using Keis.Model.Queries;

namespace Keis.Infrastructure.Features.BusinessHour.GetBusinessHour;

public class GetBusinessHourCommand : GetType, IRequest<Result<FetchBusinessHour>>
{
}

public class GetBusinessHourCommandHandler : IRequestHandler<GetBusinessHourCommand, Result<FetchBusinessHour>>
{
    private readonly IManageRepository _manageRepository;

    public GetBusinessHourCommandHandler(IManageRepository manageRepository)
    {
        _manageRepository = manageRepository;
    }

    public async Task<Result<FetchBusinessHour>> Handle(GetBusinessHourCommand request,
        CancellationToken cancellationToken)
    {
        var rst = await _manageRepository.GetBusinessHour(request.Id, cancellationToken);
        var rstHours = await _manageRepository.GetBusinessHourOpens(request.Id, cancellationToken);

        if(rst is null)
        {
           return Result.Fail<FetchBusinessHour>($"Business Hour with id [{request.Id}] does not exist!");
        }

        rst.OpenHours = rstHours.ToArray();

        return Result.Ok(rst);
    }
}