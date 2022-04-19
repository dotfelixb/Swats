using FluentResults;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Tickets.ListComments;

public class ListTicketCommentCommand : ListType, IRequest<Result<IEnumerable<FetchTicketComment>>>
{
    public string TicketId { get; set; }
}