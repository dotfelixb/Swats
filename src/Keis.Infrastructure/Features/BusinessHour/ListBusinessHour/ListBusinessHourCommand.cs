using FluentResults;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.BusinessHour.ListBusinessHour;

public class ListBusinessHourCommand : ListType, IRequest<Result<IEnumerable<FetchBusinessHour>>>
{
}