using FluentResults;
using Keis.Data.Repository;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Dashboard.CountDashboard;

public class CountDashboardCommandHandler : IRequestHandler<CountDashboardCommand, Result<DashboardCount>>
{
    private readonly ITicketRepository _ticketRepository;

    public CountDashboardCommandHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<DashboardCount>> Handle(CountDashboardCommand request, CancellationToken cancellationToken)
    {
        var result = await _ticketRepository.CountTickets(request.Id, cancellationToken);
        
        return Result.Ok(result);
    }
}