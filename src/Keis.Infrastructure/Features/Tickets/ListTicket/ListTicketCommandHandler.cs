using FluentResults;
using Keis.Data.Repository;
using Keis.Model;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Tickets.ListTicket;

public class ListTicketCommandHandler : IRequestHandler<ListTicketCommand, Result<IEnumerable<FetchTicket>>>
{
    private readonly ITicketRepository _ticketRepository;

    public ListTicketCommandHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<IEnumerable<FetchTicket>>> Handle(ListTicketCommand request,
        CancellationToken cancellationToken)
    {
        var rst = await _ticketRepository.ListTickets(
            request.Id
            , request.IncludeDepartment
            , request.IncludeTeam
            , request.IncludeHelpTopic
            , request.Offset
            , request.Limit
            , request.Deleted
            , cancellationToken);


        return Result.Ok(rst);
    }
}