﻿using FluentResults;
using MediatR;
using Keis.Data.Repository;
using Keis.Model.Commands;
using Keis.Model.Queries;

namespace Keis.Infrastructure.Features.Department.ListDepartment;

public class ListDepartmentCommand : ListType, IRequest<Result<IEnumerable<FetchDepartment>>>
{
}

public class ListDepartmentCommandHandler : IRequestHandler<ListDepartmentCommand, Result<IEnumerable<FetchDepartment>>>
{
    private readonly IManageRepository _manageRepository;

    public ListDepartmentCommandHandler(IManageRepository manageRepository)
    {
        _manageRepository = manageRepository;
    }

    public async Task<Result<IEnumerable<FetchDepartment>>> Handle(ListDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        var rst = await _manageRepository.ListDepartments(request.Offset, request.Limit, request.Deleted,
            cancellationToken);

        return Result.Ok(rst);
    }
}