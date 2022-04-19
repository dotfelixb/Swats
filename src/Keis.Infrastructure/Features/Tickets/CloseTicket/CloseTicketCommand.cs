using FluentResults;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Tickets.CloseTicket;

public class CloseTicketCommand : GetType, IRequest<Result<string>>
{
}