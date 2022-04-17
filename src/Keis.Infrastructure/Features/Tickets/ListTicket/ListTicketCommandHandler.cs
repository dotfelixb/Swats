using FluentResults;
using MediatR;
using Keis.Data.Repository;
using Keis.Model;
using Keis.Model.Commands;
using Keis.Model.Queries;

namespace Keis.Infrastructure.Features.Tickets.ListTicket;

public class ListTicketCommand : ListType, IRequest<Result<ListResult<FetchTicket>>>
{
    public string Id { get; set; }
    public bool IncludeDepartment { get; set; }
    public bool IncludeTeam { get; set; }
    public bool IncludeHelpTopic { get; set; }
}

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