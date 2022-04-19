using FluentResults;
using Keis.Data.Repository;
using Keis.Model;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Tickets.ListTicket;

public class ListTicketCommandHandler : IRequestHandler<ListTicketCommand, Result<ListResult<FetchTicket>>>
{
    private readonly ITicketRepository _ticketRepository;

    public ListTicketCommandHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<ListResult<FetchTicket>>> Handle(ListTicketCommand request,
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

        var listResult = new ListResult<FetchTicket>
        {
            Data = rst,
            Type = "list"
        };

        return Result.Ok(listResult);
    }
}