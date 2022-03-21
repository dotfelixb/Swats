using FluentResults;
using MediatR;
using Swats.Data.Repository;
using Swats.Model.Commands;
using Swats.Model.Queries;

namespace Swats.Infrastructure.Features.Tickets.ListTicket;

public class ListTicketCommandHandler : IRequestHandler<ListTicketCommand, Result<IEnumerable<FetchTicket>>>
{
    private readonly ITicketRepository _ticketRepository;

    public ListTicketCommandHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<IEnumerable<FetchTicket>>> Handle(ListTicketCommand request, CancellationToken cancellationToken)
    {
        var rst = await _ticketRepository.ListTickets(request.Offset, request.Limit, request.Deleted, cancellationToken);
        return Result.Ok(rst);
    }
}