using FluentResults;
using Keis.Data.Repository;
using MediatR;

namespace Keis.Infrastructure.Features.Tickets.CloseTicket;

public class CloseTicketCommandHandler : IRequestHandler<CloseTicketCommand, Result<string>>
{
    private readonly ITicketRepository _ticketRepository;

    public CloseTicketCommandHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public Task<Result<string>> Handle(CloseTicketCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}