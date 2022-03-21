using FluentResults;
using MediatR;
using Swats.Data.Repository;
using Swats.Model;
using Swats.Model.Commands;
using Swats.Model.Queries;

namespace Swats.Infrastructure.Features.Tickets.GetTicket;

public class GetTicketCommandHandler : IRequestHandler<GetTicketCommand, Result<FetchTicket>>
{
    private readonly ITicketRepository _ticketRepository;

    public GetTicketCommandHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<FetchTicket>> Handle(GetTicketCommand request, CancellationToken cancellationToken)
    {
        var rst = await _ticketRepository.GetTicket(request.Id, cancellationToken);

        return rst.Id.ToGuid() == Guid.Empty
            ? Result.Fail<FetchTicket>($"Ticket with id [{request.Id}] does not exist!")
            : Result.Ok(rst);
    }
}
