using FluentResults;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Department.GetDepartment;

public class GetDepartmentCommand : GetType, IRequest<Result<FetchDepartment>>
{
}