using FluentResults;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Sla.GetSla;

public class GetSlaCommand : GetType, IRequest<Result<FetchSla>>
{
}