using FluentResults;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Tickets.GetTicket;

public class GetTicketCommand : GetType, IRequest<Result<FetchTicket>>
{
}