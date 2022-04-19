using FluentResults;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.BusinessHour.GetBusinessHour;

public class GetBusinessHourCommand : GetType, IRequest<Result<FetchBusinessHour>>
{
}