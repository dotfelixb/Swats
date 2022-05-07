using FluentResults;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.TicketTypes.GetTicketType;

public class GetTicketTypeCommand : GetType, IRequest<Result<FetchTicketType>>
{
}