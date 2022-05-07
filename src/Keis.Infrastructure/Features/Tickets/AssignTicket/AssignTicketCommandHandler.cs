using FluentResults;
using Keis.Data.Repository;
using MediatR;

namespace Keis.Infrastructure.Features.Tickets.AssignTicket;

public class AssignTicketCommandHandler : IRequestHandler<AssignTicketCommand, Result<string>>
{
    private readonly ITicketRepository _ticketRepository;

    public AssignTicketCommandHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public Task<Result<string>> Handle(AssignTicketCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}