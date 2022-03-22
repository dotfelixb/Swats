using FluentResults;
using MediatR;
using Swats.Data.Repository;
using Swats.Model.Commands;

namespace Swats.Infrastructure.Features.Dashboard.AgentTickets
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
