using FluentResults;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.TicketTypes.ListTicketType;

public class ListTicketTypeCommand : ListType, IRequest<Result<IEnumerable<FetchTicketType>>>
{
}