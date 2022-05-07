using FluentResults;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Sla.ListSla;

public class ListSlaCommand : ListType, IRequest<Result<IEnumerable<FetchSla>>>
{
}