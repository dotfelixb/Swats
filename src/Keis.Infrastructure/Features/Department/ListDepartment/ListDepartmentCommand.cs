using FluentResults;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Department.ListDepartment;

public class ListDepartmentCommand : ListType, IRequest<Result<IEnumerable<FetchDepartment>>>
{
}