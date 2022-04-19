using FluentResults;
using Keis.Data.Repository;
using Keis.Model.Commands;
using MediatR;

namespace Keis.Infrastructure.Features.Dashboard.AgentTickets
{
    public class AgentTicketsCommandHandler : IRequestHandler<AgentTicketsCommand, Result<int>>
    {
        private readonly ITicketRepository _ticketRepository;

        public AgentTicketsCommandHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<Result<int>> Handle(AgentTicketsCommand request, CancellationToken cancellationToken)
        {
            var result = await _ticketRepository.CountByAgentId(request.Id, cancellationToken);

            return Result.Ok(result);
        }
    }
}