using FluentResults;
using MediatR;
using Swats.Data.Repository;
using Swats.Model.Commands;
using Swats.Model.Queries;

namespace Swats.Infrastructure.Features.Department.ListDepartment;

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